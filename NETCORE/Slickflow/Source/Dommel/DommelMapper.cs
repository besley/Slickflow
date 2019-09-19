using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Dommel
{
    /// <summary>
    /// Simple CRUD operations for Dapper.
    /// </summary>
    public static class DommelMapper
    {
        private static readonly Dictionary<string, ISqlBuilder> _sqlBuilders = new Dictionary<string, ISqlBuilder>
                                                                                {
                                                                                    { "sqlconnection", new SqlServerSqlBuilder() },
                                                                                    { "sqlceconnection", new SqlServerCeSqlBuilder() },
                                                                                    { "sqliteconnection", new SqliteSqlBuilder() },
                                                                                    { "npgsqlconnection", new PostgresSqlBuilder() },
                                                                                    { "mysqlconnection", new MySqlSqlBuilder() }
                                                                                };

        private static readonly ConcurrentDictionary<Type, string> _getQueryCache = new ConcurrentDictionary<Type, string>();
        private static readonly ConcurrentDictionary<Type, string> _getAllQueryCache = new ConcurrentDictionary<Type, string>();
        private static readonly ConcurrentDictionary<Type, string> _insertQueryCache = new ConcurrentDictionary<Type, string>();
        private static readonly ConcurrentDictionary<Type, string> _updateQueryCache = new ConcurrentDictionary<Type, string>();
        private static readonly ConcurrentDictionary<Type, string> _deleteQueryCache = new ConcurrentDictionary<Type, string>();
        private static readonly ConcurrentDictionary<Type, string> _deleteAllQueryCache = new ConcurrentDictionary<Type, string>();

        /// <summary>
        /// Retrieves the entity of type <typeparamref name="TEntity"/> with the specified id.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="id">The id of the entity in the database.</param>
        /// <returns>The entity with the corresponding id.</returns>
        public static TEntity Get<TEntity>(this IDbConnection connection, object id) where TEntity : class
        {
            DynamicParameters parameters;
            var sql = BuildGetById(typeof(TEntity), id, out parameters);
            return connection.QueryFirstOrDefault<TEntity>(sql, parameters);
        }

        /// <summary>
        /// Retrieves the entity of type <typeparamref name="TEntity"/> with the specified id.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="id">The id of the entity in the database.</param>
        /// <param name="transaction">The transaction to the connection.</param>
        /// <returns>The entity with the corresponding id.</returns>
        public static TEntity Get<TEntity>(this IDbConnection connection, object id, IDbTransaction transaction) where TEntity : class
        {
            DynamicParameters parameters;
            var sql = BuildGetById(typeof(TEntity), id, out parameters);
            return connection.QueryFirstOrDefault<TEntity>(sql, parameters, transaction);
        }

        /// <summary>
        /// Retrieves the entity of type <typeparamref name="TEntity"/> with the specified id.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="id">The id of the entity in the database.</param>
        /// <returns>The entity with the corresponding id.</returns>
        public static Task<TEntity> GetAsync<TEntity>(this IDbConnection connection, object id) where TEntity : class
        {
            DynamicParameters parameters;
            var sql = BuildGetById(typeof(TEntity), id, out parameters);
            return connection.QueryFirstOrDefaultAsync<TEntity>(sql, parameters);
        }

        private static string BuildGetById(Type type, object id, out DynamicParameters parameters)
        {
            string sql;
            if (!_getQueryCache.TryGetValue(type, out sql))
            {
                var tableName = Resolvers.Table(type);
                var keyProperty = Resolvers.KeyProperty(type);
                var keyColumnName = Resolvers.Column(keyProperty);

                sql = $"select * from {tableName} where {keyColumnName} = @Id";
                _getQueryCache.TryAdd(type, sql);
            }

            parameters = new DynamicParameters();
            parameters.Add("Id", id);

            return sql;
        }

        /// <summary>
        /// Retrieves all the entities of type <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="buffered">
        /// A value indicating whether the result of the query should be executed directly,
        /// or when the query is materialized (using <c>ToList()</c> for example).
        /// </param>
        /// <returns>A collection of entities of type <typeparamref name="TEntity"/>.</returns>
        public static IEnumerable<TEntity> GetAll<TEntity>(this IDbConnection connection, bool buffered = true) where TEntity : class
        {
            var sql = BuildGetAllQuery(typeof(TEntity));
            return connection.Query<TEntity>(sql, buffered: buffered);
        }

        /// <summary>
        /// Retrieves all the entities of type <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <returns>A collection of entities of type <typeparamref name="TEntity"/>.</returns>
        public static Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(this IDbConnection connection) where TEntity : class
        {
            var sql = BuildGetAllQuery(typeof(TEntity));
            return connection.QueryAsync<TEntity>(sql);
        }

        private static string BuildGetAllQuery(Type type)
        {
            string sql;
            if (!_getAllQueryCache.TryGetValue(type, out sql))
            {
                var tableName = Resolvers.Table(type);
                sql = $"select * from {tableName}";
                _getAllQueryCache.TryAdd(type, sql);
            }

            return sql;
        }

        /// <summary>
        /// Retrieves the entity of type <typeparamref name="TReturn"/> with the specified id
        /// joined with the types specified as type parameters.
        /// </summary>
        /// <typeparam name="T1">The first type parameter. This is the source entity.</typeparam>
        /// <typeparam name="T2">The second type parameter.</typeparam>
        /// <typeparam name="TReturn">The return type parameter.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="id">The id of the entity in the database.</param>
        /// <param name="map">The mapping to perform on the entities in the result set.</param>
        /// <returns>The entity with the corresponding id joined with the specified types.</returns>
        public static TReturn Get<T1, T2, TReturn>(this IDbConnection connection, object id, Func<T1, T2, TReturn> map) where TReturn : class
        {
            return MultiMap<T1, T2, DontMap, DontMap, DontMap, DontMap, DontMap, TReturn>(connection, map, id).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the entity of type <typeparamref name="TReturn"/> with the specified id
        /// joined with the types specified as type parameters.
        /// </summary>
        /// <typeparam name="T1">The first type parameter. This is the source entity.</typeparam>
        /// <typeparam name="T2">The second type parameter.</typeparam>
        /// <typeparam name="TReturn">The return type parameter.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="id">The id of the entity in the database.</param>
        /// <param name="map">The mapping to perform on the entities in the result set.</param>
        /// <returns>The entity with the corresponding id joined with the specified types.</returns>
        public static async Task<TReturn> GetAsync<T1, T2, TReturn>(this IDbConnection connection, object id, Func<T1, T2, TReturn> map) where TReturn : class
        {
            return (await MultiMapAsync<T1, T2, DontMap, DontMap, DontMap, DontMap, DontMap, TReturn>(connection, map, id)).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the entity of type <typeparamref name="TReturn"/> with the specified id
        /// joined with the types specified as type parameters.
        /// </summary>
        /// <typeparam name="T1">The first type parameter. This is the source entity.</typeparam>
        /// <typeparam name="T2">The second type parameter.</typeparam>
        /// <typeparam name="T3">The third type parameter.</typeparam>
        /// <typeparam name="TReturn">The return type parameter.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="id">The id of the entity in the database.</param>
        /// <param name="map">The mapping to perform on the entities in the result set.</param>
        /// <returns>The entity with the corresponding id joined with the specified types.</returns>
        public static TReturn Get<T1, T2, T3, TReturn>(this IDbConnection connection,
                                                       object id,
                                                       Func<T1, T2, T3, TReturn> map) where TReturn : class
        {
            return MultiMap<T1, T2, T3, DontMap, DontMap, DontMap, DontMap, TReturn>(connection, map, id).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the entity of type <typeparamref name="TReturn"/> with the specified id
        /// joined with the types specified as type parameters.
        /// </summary>
        /// <typeparam name="T1">The first type parameter. This is the source entity.</typeparam>
        /// <typeparam name="T2">The second type parameter.</typeparam>
        /// <typeparam name="T3">The third type parameter.</typeparam>
        /// <typeparam name="TReturn">The return type parameter.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="id">The id of the entity in the database.</param>
        /// <param name="map">The mapping to perform on the entities in the result set.</param>
        /// <returns>The entity with the corresponding id joined with the specified types.</returns>
        public static async Task<TReturn> GetAsync<T1, T2, T3, TReturn>(this IDbConnection connection,
                                                                        object id,
                                                                        Func<T1, T2, T3, TReturn> map) where TReturn : class
        {
            return (await MultiMapAsync<T1, T2, T3, DontMap, DontMap, DontMap, DontMap, TReturn>(connection, map, id)).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the entity of type <typeparamref name="TReturn"/> with the specified id
        /// joined with the types specified as type parameters.
        /// </summary>
        /// <typeparam name="T1">The first type parameter. This is the source entity.</typeparam>
        /// <typeparam name="T2">The second type parameter.</typeparam>
        /// <typeparam name="T3">The third type parameter.</typeparam>
        /// <typeparam name="T4">The fourth type parameter.</typeparam>
        /// <typeparam name="TReturn">The return type parameter.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="id">The id of the entity in the database.</param>
        /// <param name="map">The mapping to perform on the entities in the result set.</param>
        /// <returns>The entity with the corresponding id joined with the specified types.</returns>
        public static TReturn Get<T1, T2, T3, T4, TReturn>(this IDbConnection connection,
                                                           object id,
                                                           Func<T1, T2, T3, T4, TReturn> map) where TReturn : class
        {
            return MultiMap<T1, T2, T3, T4, DontMap, DontMap, DontMap, TReturn>(connection, map, id).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the entity of type <typeparamref name="TReturn"/> with the specified id
        /// joined with the types specified as type parameters.
        /// </summary>
        /// <typeparam name="T1">The first type parameter. This is the source entity.</typeparam>
        /// <typeparam name="T2">The second type parameter.</typeparam>
        /// <typeparam name="T3">The third type parameter.</typeparam>
        /// <typeparam name="T4">The fourth type parameter.</typeparam>
        /// <typeparam name="TReturn">The return type parameter.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="id">The id of the entity in the database.</param>
        /// <param name="map">The mapping to perform on the entities in the result set.</param>
        /// <returns>The entity with the corresponding id joined with the specified types.</returns>
        public static async Task<TReturn> GetAsync<T1, T2, T3, T4, TReturn>(this IDbConnection connection,
                                                                            object id,
                                                                            Func<T1, T2, T3, T4, TReturn> map) where TReturn : class
        {
            return (await MultiMapAsync<T1, T2, T3, T4, DontMap, DontMap, DontMap, TReturn>(connection, map, id)).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the entity of type <typeparamref name="TReturn"/> with the specified id
        /// joined with the types specified as type parameters.
        /// </summary>
        /// <typeparam name="T1">The first type parameter. This is the source entity.</typeparam>
        /// <typeparam name="T2">The second type parameter.</typeparam>
        /// <typeparam name="T3">The third type parameter.</typeparam>
        /// <typeparam name="T4">The fourth type parameter.</typeparam>
        /// <typeparam name="T5">The fifth type parameter.</typeparam>
        /// <typeparam name="TReturn">The return type parameter.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="id">The id of the entity in the database.</param>
        /// <param name="map">The mapping to perform on the entities in the result set.</param>
        /// <returns>The entity with the corresponding id joined with the specified types.</returns>
        public static TReturn Get<T1, T2, T3, T4, T5, TReturn>(this IDbConnection connection,
                                                               object id,
                                                               Func<T1, T2, T3, T4, T5, TReturn> map) where TReturn : class
        {
            return MultiMap<T1, T2, T3, T4, T5, DontMap, DontMap, TReturn>(connection, map, id).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the entity of type <typeparamref name="TReturn"/> with the specified id
        /// joined with the types specified as type parameters.
        /// </summary>
        /// <typeparam name="T1">The first type parameter. This is the source entity.</typeparam>
        /// <typeparam name="T2">The second type parameter.</typeparam>
        /// <typeparam name="T3">The third type parameter.</typeparam>
        /// <typeparam name="T4">The fourth type parameter.</typeparam>
        /// <typeparam name="T5">The fifth type parameter.</typeparam>
        /// <typeparam name="TReturn">The return type parameter.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="id">The id of the entity in the database.</param>
        /// <param name="map">The mapping to perform on the entities in the result set.</param>
        /// <returns>The entity with the corresponding id joined with the specified types.</returns>
        public static async Task<TReturn> GetAsync<T1, T2, T3, T4, T5, TReturn>(this IDbConnection connection,
                                                                                object id,
                                                                                Func<T1, T2, T3, T4, T5, TReturn> map) where TReturn : class
        {
            return (await MultiMapAsync<T1, T2, T3, T4, T5, DontMap, DontMap, TReturn>(connection, map, id)).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the entity of type <typeparamref name="TReturn"/> with the specified id
        /// joined with the types specified as type parameters.
        /// </summary>
        /// <typeparam name="T1">The first type parameter. This is the source entity.</typeparam>
        /// <typeparam name="T2">The second type parameter.</typeparam>
        /// <typeparam name="T3">The third type parameter.</typeparam>
        /// <typeparam name="T4">The fourth type parameter.</typeparam>
        /// <typeparam name="T5">The fifth type parameter.</typeparam>
        /// <typeparam name="T6">The sixth type parameter.</typeparam>
        /// <typeparam name="TReturn">The return type parameter.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="id">The id of the entity in the database.</param>
        /// <param name="map">The mapping to perform on the entities in the result set.</param>
        /// <returns>The entity with the corresponding id joined with the specified types.</returns>
        public static TReturn Get<T1, T2, T3, T4, T5, T6, TReturn>(this IDbConnection connection,
                                                                   object id,
                                                                   Func<T1, T2, T3, T4, T5, T6, TReturn> map) where TReturn : class
        {
            return MultiMap<T1, T2, T3, T4, T5, T6, DontMap, TReturn>(connection, map, id).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the entity of type <typeparamref name="TReturn"/> with the specified id
        /// joined with the types specified as type parameters.
        /// </summary>
        /// <typeparam name="T1">The first type parameter. This is the source entity.</typeparam>
        /// <typeparam name="T2">The second type parameter.</typeparam>
        /// <typeparam name="T3">The third type parameter.</typeparam>
        /// <typeparam name="T4">The fourth type parameter.</typeparam>
        /// <typeparam name="T5">The fifth type parameter.</typeparam>
        /// <typeparam name="T6">The sixth type parameter.</typeparam>
        /// <typeparam name="TReturn">The return type parameter.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="id">The id of the entity in the database.</param>
        /// <param name="map">The mapping to perform on the entities in the result set.</param>
        /// <returns>The entity with the corresponding id joined with the specified types.</returns>
        public static async Task<TReturn> GetAsync<T1, T2, T3, T4, T5, T6, TReturn>(this IDbConnection connection,
                                                                                    object id,
                                                                                    Func<T1, T2, T3, T4, T5, T6, TReturn> map) where TReturn : class
        {
            return (await MultiMapAsync<T1, T2, T3, T4, T5, T6, DontMap, TReturn>(connection, map, id)).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the entity of type <typeparamref name="TReturn"/> with the specified id
        /// joined with the types specified as type parameters.
        /// </summary>
        /// <typeparam name="T1">The first type parameter. This is the source entity.</typeparam>
        /// <typeparam name="T2">The second type parameter.</typeparam>
        /// <typeparam name="T3">The third type parameter.</typeparam>
        /// <typeparam name="T4">The fourth type parameter.</typeparam>
        /// <typeparam name="T5">The fifth type parameter.</typeparam>
        /// <typeparam name="T6">The sixth type parameter.</typeparam>
        /// <typeparam name="T7">The seventh type parameter.</typeparam>
        /// <typeparam name="TReturn">The return type parameter.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="id">The id of the entity in the database.</param>
        /// <param name="map">The mapping to perform on the entities in the result set.</param>
        /// <returns>The entity with the corresponding id joined with the specified types.</returns>
        public static TReturn Get<T1, T2, T3, T4, T5, T6, T7, TReturn>(this IDbConnection connection,
                                                                       object id,
                                                                       Func<T1, T2, T3, T4, T5, T6, T7, TReturn> map) where TReturn : class
        {
            return MultiMap<T1, T2, T3, T4, T5, T6, T7, TReturn>(connection, map, id).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the entity of type <typeparamref name="TReturn"/> with the specified id
        /// joined with the types specified as type parameters.
        /// </summary>
        /// <typeparam name="T1">The first type parameter. This is the source entity.</typeparam>
        /// <typeparam name="T2">The second type parameter.</typeparam>
        /// <typeparam name="T3">The third type parameter.</typeparam>
        /// <typeparam name="T4">The fourth type parameter.</typeparam>
        /// <typeparam name="T5">The fifth type parameter.</typeparam>
        /// <typeparam name="T6">The sixth type parameter.</typeparam>
        /// <typeparam name="T7">The seventh type parameter.</typeparam>
        /// <typeparam name="TReturn">The return type parameter.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="id">The id of the entity in the database.</param>
        /// <param name="map">The mapping to perform on the entities in the result set.</param>
        /// <returns>The entity with the corresponding id joined with the specified types.</returns>
        public static async Task<TReturn> GetAsync<T1, T2, T3, T4, T5, T6, T7, TReturn>(this IDbConnection connection,
                                                                                        object id,
                                                                                        Func<T1, T2, T3, T4, T5, T6, T7, TReturn> map) where TReturn : class
        {
            return (await MultiMapAsync<T1, T2, T3, T4, T5, T6, T7, TReturn>(connection, map, id)).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves all the entities of type <typeparamref name="TReturn"/>
        /// joined with the types specified as type parameters.
        /// </summary>
        /// <typeparam name="T1">The first type parameter. This is the source entity.</typeparam>
        /// <typeparam name="T2">The second type parameter.</typeparam>
        /// <typeparam name="TReturn">The return type parameter.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="map">The mapping to perform on the entities in the result set.</param>
        /// <param name="buffered">
        /// A value indicating whether the result of the query should be executed directly,
        /// or when the query is materialized (using <c>ToList()</c> for example).
        /// </param>
        /// <returns>
        /// A collection of entities of type <typeparamref name="TReturn"/>
        /// joined with the specified type types.
        /// </returns>
        public static IEnumerable<TReturn> GetAll<T1, T2, TReturn>(this IDbConnection connection, Func<T1, T2, TReturn> map, bool buffered = true)
        {
            return MultiMap<T1, T2, DontMap, DontMap, DontMap, DontMap, DontMap, TReturn>(connection, map, buffered: buffered);
        }

        /// <summary>
        /// Retrieves all the entities of type <typeparamref name="TReturn"/>
        /// joined with the types specified as type parameters.
        /// </summary>
        /// <typeparam name="T1">The first type parameter. This is the source entity.</typeparam>
        /// <typeparam name="T2">The second type parameter.</typeparam>
        /// <typeparam name="TReturn">The return type parameter.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="map">The mapping to perform on the entities in the result set.</param>
        /// <param name="buffered">
        /// A value indicating whether the result of the query should be executed directly,
        /// or when the query is materialized (using <c>ToList()</c> for example).
        /// </param>
        /// <returns>
        /// A collection of entities of type <typeparamref name="TReturn"/>
        /// joined with the specified type types.
        /// </returns>
        public static Task<IEnumerable<TReturn>> GetAllAsync<T1, T2, TReturn>(this IDbConnection connection, Func<T1, T2, TReturn> map, bool buffered = true)
        {
            return MultiMapAsync<T1, T2, DontMap, DontMap, DontMap, DontMap, DontMap, TReturn>(connection, map, buffered: buffered);
        }

        /// <summary>
        /// Retrieves all the entities of type <typeparamref name="TReturn"/>
        /// joined with the types specified as type parameters.
        /// </summary>
        /// <typeparam name="T1">The first type parameter. This is the source entity.</typeparam>
        /// <typeparam name="T2">The second type parameter.</typeparam>
        /// <typeparam name="T3">The third type parameter.</typeparam>
        /// <typeparam name="TReturn">The return type parameter.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="map">The mapping to perform on the entities in the result set.</param>
        /// <param name="buffered">
        /// A value indicating whether the result of the query should be executed directly,
        /// or when the query is materialized (using <c>ToList()</c> for example).
        /// </param>
        /// <returns>
        /// A collection of entities of type <typeparamref name="TReturn"/>
        /// joined with the specified type types.
        /// </returns>
        public static IEnumerable<TReturn> GetAll<T1, T2, T3, TReturn>(this IDbConnection connection, Func<T1, T2, T3, TReturn> map, bool buffered = true)
        {
            return MultiMap<T1, T2, T3, DontMap, DontMap, DontMap, DontMap, TReturn>(connection, map, buffered: buffered);
        }

        /// <summary>
        /// Retrieves all the entities of type <typeparamref name="TReturn"/>
        /// joined with the types specified as type parameters.
        /// </summary>
        /// <typeparam name="T1">The first type parameter. This is the source entity.</typeparam>
        /// <typeparam name="T2">The second type parameter.</typeparam>
        /// <typeparam name="T3">The third type parameter.</typeparam>
        /// <typeparam name="TReturn">The return type parameter.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="map">The mapping to perform on the entities in the result set.</param>
        /// <param name="buffered">
        /// A value indicating whether the result of the query should be executed directly,
        /// or when the query is materialized (using <c>ToList()</c> for example).
        /// </param>
        /// <returns>
        /// A collection of entities of type <typeparamref name="TReturn"/>
        /// joined with the specified type types.
        /// </returns>
        public static Task<IEnumerable<TReturn>> GetAllAsync<T1, T2, T3, TReturn>(this IDbConnection connection, Func<T1, T2, T3, TReturn> map, bool buffered = true)
        {
            return MultiMapAsync<T1, T2, T3, DontMap, DontMap, DontMap, DontMap, TReturn>(connection, map, buffered: buffered);
        }

        /// <summary>
        /// Retrieves all the entities of type <typeparamref name="TReturn"/>
        /// joined with the types specified as type parameters.
        /// </summary>
        /// <typeparam name="T1">The first type parameter. This is the source entity.</typeparam>
        /// <typeparam name="T2">The second type parameter.</typeparam>
        /// <typeparam name="T3">The third type parameter.</typeparam>
        /// <typeparam name="T4">The fourth type parameter.</typeparam>
        /// <typeparam name="TReturn">The return type parameter.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="map">The mapping to perform on the entities in the result set.</param>
        /// <param name="buffered">
        /// A value indicating whether the result of the query should be executed directly,
        /// or when the query is materialized (using <c>ToList()</c> for example).
        /// </param>
        /// <returns>
        /// A collection of entities of type <typeparamref name="TReturn"/>
        /// joined with the specified type types.
        /// </returns>
        public static IEnumerable<TReturn> GetAll<T1, T2, T3, T4, TReturn>(this IDbConnection connection, Func<T1, T2, T3, T4, TReturn> map, bool buffered = true)
        {
            return MultiMap<T1, T2, T3, T4, DontMap, DontMap, DontMap, TReturn>(connection, map, buffered: buffered);
        }

        /// <summary>
        /// Retrieves all the entities of type <typeparamref name="TReturn"/>
        /// joined with the types specified as type parameters.
        /// </summary>
        /// <typeparam name="T1">The first type parameter. This is the source entity.</typeparam>
        /// <typeparam name="T2">The second type parameter.</typeparam>
        /// <typeparam name="T3">The third type parameter.</typeparam>
        /// <typeparam name="T4">The fourth type parameter.</typeparam>
        /// <typeparam name="TReturn">The return type parameter.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="map">The mapping to perform on the entities in the result set.</param>
        /// <param name="buffered">
        /// A value indicating whether the result of the query should be executed directly,
        /// or when the query is materialized (using <c>ToList()</c> for example).
        /// </param>
        /// <returns>
        /// A collection of entities of type <typeparamref name="TReturn"/>
        /// joined with the specified type types.
        /// </returns>
        public static Task<IEnumerable<TReturn>> GetAllAsync<T1, T2, T3, T4, TReturn>(this IDbConnection connection, Func<T1, T2, T3, T4, TReturn> map, bool buffered = true)
        {
            return MultiMapAsync<T1, T2, T3, T4, DontMap, DontMap, DontMap, TReturn>(connection, map, buffered: buffered);
        }

        /// <summary>
        /// Retrieves all the entities of type <typeparamref name="TReturn"/>
        /// joined with the types specified as type parameters.
        /// </summary>
        /// <typeparam name="T1">The first type parameter. This is the source entity.</typeparam>
        /// <typeparam name="T2">The second type parameter.</typeparam>
        /// <typeparam name="T3">The third type parameter.</typeparam>
        /// <typeparam name="T4">The fourth type parameter.</typeparam>
        /// <typeparam name="T5">The fifth type parameter.</typeparam>
        /// <typeparam name="TReturn">The return type parameter.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="map">The mapping to perform on the entities in the result set.</param>
        /// <param name="buffered">
        /// A value indicating whether the result of the query should be executed directly,
        /// or when the query is materialized (using <c>ToList()</c> for example).
        /// </param>
        /// <returns>
        /// A collection of entities of type <typeparamref name="TReturn"/>
        /// joined with the specified type types.
        /// </returns>
        public static IEnumerable<TReturn> GetAll<T1, T2, T3, T4, T5, TReturn>(this IDbConnection connection, Func<T1, T2, T3, T4, T5, TReturn> map, bool buffered = true)
        {
            return MultiMap<T1, T2, T3, T4, T5, DontMap, DontMap, TReturn>(connection, map, buffered: buffered);
        }

        /// <summary>
        /// Retrieves all the entities of type <typeparamref name="TReturn"/>
        /// joined with the types specified as type parameters.
        /// </summary>
        /// <typeparam name="T1">The first type parameter. This is the source entity.</typeparam>
        /// <typeparam name="T2">The second type parameter.</typeparam>
        /// <typeparam name="T3">The third type parameter.</typeparam>
        /// <typeparam name="T4">The fourth type parameter.</typeparam>
        /// <typeparam name="T5">The fifth type parameter.</typeparam>
        /// <typeparam name="TReturn">The return type parameter.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="map">The mapping to perform on the entities in the result set.</param>
        /// <param name="buffered">
        /// A value indicating whether the result of the query should be executed directly,
        /// or when the query is materialized (using <c>ToList()</c> for example).
        /// </param>
        /// <returns>
        /// A collection of entities of type <typeparamref name="TReturn"/>
        /// joined with the specified type types.
        /// </returns>
        public static Task<IEnumerable<TReturn>> GetAllAsync<T1, T2, T3, T4, T5, TReturn>(this IDbConnection connection, Func<T1, T2, T3, T4, T5, TReturn> map, bool buffered = true)
        {
            return MultiMapAsync<T1, T2, T3, T4, T5, DontMap, DontMap, TReturn>(connection, map, buffered: buffered);
        }

        /// <summary>
        /// Retrieves all the entities of type <typeparamref name="TReturn"/>
        /// joined with the types specified as type parameters.
        /// </summary>
        /// <typeparam name="T1">The first type parameter. This is the source entity.</typeparam>
        /// <typeparam name="T2">The second type parameter.</typeparam>
        /// <typeparam name="T3">The third type parameter.</typeparam>
        /// <typeparam name="T4">The fourth type parameter.</typeparam>
        /// <typeparam name="T5">The fifth type parameter.</typeparam>
        /// <typeparam name="T6">The sixth type parameter.</typeparam>
        /// <typeparam name="TReturn">The return type parameter.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="map">The mapping to perform on the entities in the result set.</param>
        /// <param name="buffered">
        /// A value indicating whether the result of the query should be executed directly,
        /// or when the query is materialized (using <c>ToList()</c> for example).
        /// </param>
        /// <returns>
        /// A collection of entities of type <typeparamref name="TReturn"/>
        /// joined with the specified type types.
        /// </returns>
        public static IEnumerable<TReturn> GetAll<T1, T2, T3, T4, T5, T6, TReturn>(this IDbConnection connection, Func<T1, T2, T3, T4, T5, T6, TReturn> map, bool buffered = true)
        {
            return MultiMap<T1, T2, T3, T4, T5, T6, DontMap, TReturn>(connection, map, buffered: buffered);
        }

        /// <summary>
        /// Retrieves all the entities of type <typeparamref name="TReturn"/>
        /// joined with the types specified as type parameters.
        /// </summary>
        /// <typeparam name="T1">The first type parameter. This is the source entity.</typeparam>
        /// <typeparam name="T2">The second type parameter.</typeparam>
        /// <typeparam name="T3">The third type parameter.</typeparam>
        /// <typeparam name="T4">The fourth type parameter.</typeparam>
        /// <typeparam name="T5">The fifth type parameter.</typeparam>
        /// <typeparam name="T6">The sixth type parameter.</typeparam>
        /// <typeparam name="TReturn">The return type parameter.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="map">The mapping to perform on the entities in the result set.</param>
        /// <param name="buffered">
        /// A value indicating whether the result of the query should be executed directly,
        /// or when the query is materialized (using <c>ToList()</c> for example).
        /// </param>
        /// <returns>
        /// A collection of entities of type <typeparamref name="TReturn"/>
        /// joined with the specified type types.
        /// </returns>
        public static Task<IEnumerable<TReturn>> GetAllAsync<T1, T2, T3, T4, T5, T6, TReturn>(this IDbConnection connection, Func<T1, T2, T3, T4, T5, T6, TReturn> map, bool buffered = true)
        {
            return MultiMapAsync<T1, T2, T3, T4, T5, T6, DontMap, TReturn>(connection, map, buffered: buffered);
        }

        /// <summary>
        /// Retrieves all the entities of type <typeparamref name="TReturn"/>
        /// joined with the types specified as type parameters.
        /// </summary>
        /// <typeparam name="T1">The first type parameter. This is the source entity.</typeparam>
        /// <typeparam name="T2">The second type parameter.</typeparam>
        /// <typeparam name="T3">The third type parameter.</typeparam>
        /// <typeparam name="T4">The fourth type parameter.</typeparam>
        /// <typeparam name="T5">The fifth type parameter.</typeparam>
        /// <typeparam name="T6">The sixth type parameter.</typeparam>
        /// <typeparam name="T7">The seventh type parameter.</typeparam>
        /// <typeparam name="TReturn">The return type parameter.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="map">The mapping to perform on the entities in the result set.</param>
        /// <param name="buffered">
        /// A value indicating whether the result of the query should be executed directly,
        /// or when the query is materialized (using <c>ToList()</c> for example).
        /// </param>
        /// <returns>
        /// A collection of entities of type <typeparamref name="TReturn"/>
        /// joined with the specified type types.
        /// </returns>
        public static IEnumerable<TReturn> GetAll<T1, T2, T3, T4, T5, T6, T7, TReturn>(this IDbConnection connection, Func<T1, T2, T3, T4, T5, T6, T7, TReturn> map, bool buffered = true)
        {
            return MultiMap<T1, T2, T3, T4, T5, T6, T7, TReturn>(connection, map, buffered: buffered);
        }

        /// <summary>
        /// Retrieves all the entities of type <typeparamref name="TReturn"/>
        /// joined with the types specified as type parameters.
        /// </summary>
        /// <typeparam name="T1">The first type parameter. This is the source entity.</typeparam>
        /// <typeparam name="T2">The second type parameter.</typeparam>
        /// <typeparam name="T3">The third type parameter.</typeparam>
        /// <typeparam name="T4">The fourth type parameter.</typeparam>
        /// <typeparam name="T5">The fifth type parameter.</typeparam>
        /// <typeparam name="T6">The sixth type parameter.</typeparam>
        /// <typeparam name="T7">The seventh type parameter.</typeparam>
        /// <typeparam name="TReturn">The return type parameter.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="map">The mapping to perform on the entities in the result set.</param>
        /// <param name="buffered">
        /// A value indicating whether the result of the query should be executed directly,
        /// or when the query is materialized (using <c>ToList()</c> for example).
        /// </param>
        /// <returns>
        /// A collection of entities of type <typeparamref name="TReturn"/>
        /// joined with the specified type types.
        /// </returns>
        public static Task<IEnumerable<TReturn>> GetAllAsync<T1, T2, T3, T4, T5, T6, T7, TReturn>(this IDbConnection connection, Func<T1, T2, T3, T4, T5, T6, T7, TReturn> map, bool buffered = true)
        {
            return MultiMapAsync<T1, T2, T3, T4, T5, T6, T7, TReturn>(connection, map, buffered: buffered);
        }

        private static IEnumerable<TReturn> MultiMap<T1, T2, T3, T4, T5, T6, T7, TReturn>(IDbConnection connection, Delegate map, object id = null, bool buffered = true)
        {
            var resultType = typeof(TReturn);
            var
                includeTypes = new[]
                               {
                                   typeof(T1),
                                   typeof(T2),
                                   typeof(T3),
                                   typeof(T4),
                                   typeof(T5),
                                   typeof(T6),
                                   typeof(T7)
                               }
                    .Where(t => t != typeof(DontMap))
                    .ToArray();

            DynamicParameters parameters;
            var sql = BuildMultiMapQuery(resultType, includeTypes, id, out parameters);

            switch (includeTypes.Length)
            {
                case 2:
                    return connection.Query(sql, (Func<T1, T2, TReturn>)map, parameters, buffered: buffered);
                case 3:
                    return connection.Query(sql, (Func<T1, T2, T3, TReturn>)map, parameters, buffered: buffered);
                case 4:
                    return connection.Query(sql, (Func<T1, T2, T3, T4, TReturn>)map, parameters, buffered: buffered);
                case 5:
                    return connection.Query(sql, (Func<T1, T2, T3, T4, T5, TReturn>)map, parameters, buffered: buffered);
                case 6:
                    return connection.Query(sql, (Func<T1, T2, T3, T4, T5, T6, TReturn>)map, parameters, buffered: buffered);
                case 7:
                    return connection.Query(sql, (Func<T1, T2, T3, T4, T5, T6, T7, TReturn>)map, parameters, buffered: buffered);
            }

            throw new InvalidOperationException($"Invalid amount of include types: {includeTypes.Length}.");
        }

        private static Task<IEnumerable<TReturn>> MultiMapAsync<T1, T2, T3, T4, T5, T6, T7, TReturn>(IDbConnection connection, Delegate map, object id = null, bool buffered = true)
        {
            var resultType = typeof(TReturn);
            var
                includeTypes = new[]
                               {
                                   typeof(T1),
                                   typeof(T2),
                                   typeof(T3),
                                   typeof(T4),
                                   typeof(T5),
                                   typeof(T6),
                                   typeof(T7)
                               }
                    .Where(t => t != typeof(DontMap))
                    .ToArray();

            DynamicParameters parameters;
            var sql = BuildMultiMapQuery(resultType, includeTypes, id, out parameters);

            switch (includeTypes.Length)
            {
                case 2:
                    return connection.QueryAsync(sql, (Func<T1, T2, TReturn>)map, parameters, buffered: buffered);
                case 3:
                    return connection.QueryAsync(sql, (Func<T1, T2, T3, TReturn>)map, parameters, buffered: buffered);
                case 4:
                    return connection.QueryAsync(sql, (Func<T1, T2, T3, T4, TReturn>)map, parameters, buffered: buffered);
                case 5:
                    return connection.QueryAsync(sql, (Func<T1, T2, T3, T4, T5, TReturn>)map, parameters, buffered: buffered);
                case 6:
                    return connection.QueryAsync(sql, (Func<T1, T2, T3, T4, T5, T6, TReturn>)map, parameters, buffered: buffered);
                case 7:
                    return connection.QueryAsync(sql, (Func<T1, T2, T3, T4, T5, T6, T7, TReturn>)map, parameters, buffered: buffered);
            }

            throw new InvalidOperationException($"Invalid amount of include types: {includeTypes.Length}.");
        }

        private static string BuildMultiMapQuery(Type resultType, Type[] includeTypes, object id, out DynamicParameters parameters)
        {
            var resultTableName = Resolvers.Table(resultType);
            var resultTableKeyColumnName = Resolvers.Column(Resolvers.KeyProperty(resultType));

            var sql = $"select * from {resultTableName}";

            for (var i = 1; i < includeTypes.Length; i++)
            {
                // Determine the table to join with.
                var sourceType = includeTypes[i - 1];
                var sourceTableName = Resolvers.Table(sourceType);

                // Determine the table name of the joined table.
                var includeType = includeTypes[i];
                var foreignKeyTableName = Resolvers.Table(includeType);

                // Determine the foreign key and the relationship type.
                ForeignKeyRelation relation;
                var foreignKeyProperty = Resolvers.ForeignKeyProperty(sourceType, includeType, out relation);
                var foreignKeyPropertyName = Resolvers.Column(foreignKeyProperty);

                // If the foreign key property is nullable, use a left-join.
                var joinType = Nullable.GetUnderlyingType(foreignKeyProperty.PropertyType) != null
                                   ? "left"
                                   : "inner";

                switch (relation)
                {
                    case ForeignKeyRelation.OneToOne:
                        // Determine the primary key of the foreign key table.
                        var foreignKeyTableKeyColumName = Resolvers.Column(Resolvers.KeyProperty(includeType));

                        sql += string.Format(" {0} join {1} on {2}.{3} = {1}.{4}",
                                             joinType,
                                             foreignKeyTableName,
                                             sourceTableName,
                                             foreignKeyPropertyName,
                                             foreignKeyTableKeyColumName);
                        break;

                    case ForeignKeyRelation.OneToMany:
                        // Determine the primary key of the source table.
                        var sourceKeyColumnName = Resolvers.Column(Resolvers.KeyProperty(sourceType));

                        sql += string.Format(" {0} join {1} on {2}.{3} = {1}.{4}",
                                             joinType,
                                             foreignKeyTableName,
                                             sourceTableName,
                                             sourceKeyColumnName,
                                             foreignKeyPropertyName);
                        break;

                    case ForeignKeyRelation.ManyToMany:
                        throw new NotImplementedException("Many-to-many relationships are not supported yet.");

                    default:
                        throw new NotImplementedException($"Foreign key relation type '{relation}' is not implemented.");
                }
            }

            parameters = null;
            if (id != null)
            {
                sql += string.Format(" where {0}.{1} = @{1}", resultTableName, resultTableKeyColumnName);

                parameters = new DynamicParameters();
                parameters.Add("Id", id);
            }

            return sql;
        }

        private class DontMap
        {
        }

        /// <summary>
        /// Selects all the entities matching the specified predicate.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="predicate">A predicate to filter the results.</param>
        /// <param name="buffered">
        /// A value indicating whether the result of the query should be executed directly,
        /// or when the query is materialized (using <c>ToList()</c> for example).
        /// </param>
        /// <returns>
        /// A collection of entities of type <typeparamref name="TEntity"/> matching the specified
        /// <paramref name="predicate"/>.
        /// </returns>
        public static IEnumerable<TEntity> Select<TEntity>(this IDbConnection connection, Expression<Func<TEntity, bool>> predicate, bool buffered = true)
        {
            DynamicParameters parameters;
            var sql = BuildSelectSql(predicate, out parameters);
            return connection.Query<TEntity>(sql, parameters, buffered: buffered);
        }

        /// <summary>
        /// Selects all the entities matching the specified predicate.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="predicate">A predicate to filter the results.</param>
        /// <returns>
        /// A collection of entities of type <typeparamref name="TEntity"/> matching the specified
        /// <paramref name="predicate"/>.
        /// </returns>
        public static Task<IEnumerable<TEntity>> SelectAsync<TEntity>(this IDbConnection connection, Expression<Func<TEntity, bool>> predicate)
        {
            DynamicParameters parameters;
            var sql = BuildSelectSql(predicate, out parameters);
            return connection.QueryAsync<TEntity>(sql, parameters);
        }

        private static string BuildSelectSql<TEntity>(Expression<Func<TEntity, bool>> predicate, out DynamicParameters parameters)
        {
            var type = typeof(TEntity);
            string sql;
            if (!_getAllQueryCache.TryGetValue(type, out sql))
            {
                var tableName = Resolvers.Table(type);
                sql = $"select * from {tableName}";
                _getAllQueryCache.TryAdd(type, sql);
            }

            sql += new SqlExpression<TEntity>()
                .Where(predicate)
                .ToSql(out parameters);
            return sql;
        }

        /// <summary>
        /// Selects the first entity matching the specified predicate, or a default value if no entity matched.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="predicate">A predicate to filter the results.</param>
        /// <returns>
        /// A instance of type <typeparamref name="TEntity"/> matching the specified
        /// <paramref name="predicate"/>.
        /// </returns>
        public static TEntity FirstOrDefault<TEntity>(this IDbConnection connection, Expression<Func<TEntity, bool>> predicate)
        {
            DynamicParameters parameters;
            var sql = BuildSelectSql(predicate, out parameters);
            return connection.QueryFirstOrDefault<TEntity>(sql, parameters);
        }

        /// <summary>
        /// Selects the first entity matching the specified predicate, or a default value if no entity matched.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="predicate">A predicate to filter the results.</param>
        /// <returns>
        /// A instance of type <typeparamref name="TEntity"/> matching the specified
        /// <paramref name="predicate"/>.
        /// </returns>
        public static Task<TEntity> FirstOrDefaultAsync<TEntity>(this IDbConnection connection, Expression<Func<TEntity, bool>> predicate)
        {
            DynamicParameters parameters;
            var sql = BuildSelectSql(predicate, out parameters);
            return connection.QueryFirstOrDefaultAsync<TEntity>(sql, parameters);
        }

        /// <summary>
        /// Represents a typed SQL expression.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        public class SqlExpression<TEntity>
        {
            private readonly StringBuilder _whereBuilder = new StringBuilder();
            private readonly DynamicParameters _parameters = new DynamicParameters();
            private int _parameterIndex;

            /// <summary>
            /// Builds a SQL expression for the specified filter expression.
            /// </summary>
            /// <param name="expression">The filter expression on the entity.</param>
            /// <returns>The current <see cref="DommelMapper.SqlExpression&lt;TEntity&gt;"/> instance.</returns>
            public virtual SqlExpression<TEntity> Where(Expression<Func<TEntity, bool>> expression)
            {
                if (expression != null)
                {
                    AppendToWhere("and", expression);
                }
                return this;
            }

            private void AppendToWhere(string conditionOperator, Expression expression)
            {
                var sqlExpression = VisitExpression(expression).ToString();
                AppendToWhere(conditionOperator, sqlExpression);
            }

            private void AppendToWhere(string conditionOperator, string sqlExpression)
            {
                if (_whereBuilder.Length == 0)
                {
                    _whereBuilder.Append(" where ");
                }
                else
                {
                    _whereBuilder.AppendFormat(" {0} ", conditionOperator);
                }

                _whereBuilder.Append(sqlExpression);
            }

            /// <summary>
            /// Visits the expression.
            /// </summary>
            /// <param name="expression">The expression to visit.</param>
            /// <returns>The result of the visit.</returns>
            protected virtual object VisitExpression(Expression expression)
            {
                switch (expression.NodeType)
                {
                    case ExpressionType.Lambda:
                        return VisitLambda(expression as LambdaExpression);

                    case ExpressionType.LessThan:
                    case ExpressionType.LessThanOrEqual:
                    case ExpressionType.GreaterThan:
                    case ExpressionType.GreaterThanOrEqual:
                    case ExpressionType.Equal:
                    case ExpressionType.NotEqual:
                    case ExpressionType.And:
                    case ExpressionType.AndAlso:
                    case ExpressionType.Or:
                    case ExpressionType.OrElse:
                        return VisitBinary((BinaryExpression)expression);

                    case ExpressionType.Convert:
                    case ExpressionType.Not:
                        return VisitUnary((UnaryExpression)expression);

                    case ExpressionType.New:
                        return VisitNew((NewExpression)expression);

                    case ExpressionType.MemberAccess:
                        return VisitMemberAccess((MemberExpression)expression);

                    case ExpressionType.Constant:
                        return VisitConstantExpression((ConstantExpression)expression);
                }

                return expression;
            }

            /// <summary>
            /// Processes a lambda expression.
            /// </summary>
            /// <param name="epxression">The lambda expression.</param>
            /// <returns>The result of the processing.</returns>
            protected virtual object VisitLambda(LambdaExpression epxression)
            {
                if (epxression.Body.NodeType == ExpressionType.MemberAccess)
                {
                    var member = epxression.Body as MemberExpression;
                    if (member?.Expression != null)
                    {
                        return $"{VisitMemberAccess(member)} = '1'";
                    }
                }

                return VisitExpression(epxression.Body);
            }

            /// <summary>
            /// Processes a binary expression.
            /// </summary>
            /// <param name="expression">The binary expression.</param>
            /// <returns>The result of the processing.</returns>
            protected virtual object VisitBinary(BinaryExpression expression)
            {
                object left, right;
                var operand = BindOperant(expression.NodeType);
                if (operand == "and" || operand == "or")
                {
                    // Left side.
                    var member = expression.Left as MemberExpression;
                    if (member != null &&
                        member.Expression != null &&
                        member.Expression.NodeType == ExpressionType.Parameter)
                    {
                        left = $"{VisitMemberAccess(member)} = '1'";
                    }
                    else
                    {
                        left = VisitExpression(expression.Left);
                    }

                    // Right side.
                    member = expression.Right as MemberExpression;
                    if (member != null &&
                        member.Expression != null &&
                        member.Expression.NodeType == ExpressionType.Parameter)
                    {
                        right = $"{VisitMemberAccess(member)} = '1'";
                    }
                    else
                    {
                        right = VisitExpression(expression.Right);
                    }
                }
                else
                {
                    // It's a single expression.
                    left = VisitExpression(expression.Left);
                    right = VisitExpression(expression.Right);

                    var paramName = "p" + _parameterIndex++;
                    _parameters.Add(paramName, value: right);
                    return $"{left} {operand} @{paramName}";
                }

                return $"{left} {operand} {right}";
            }

            /// <summary>
            /// Processes a unary expression.
            /// </summary>
            /// <param name="expression">The unary expression.</param>
            /// <returns>The result of the processing.</returns>
            protected virtual object VisitUnary(UnaryExpression expression)
            {
                switch (expression.NodeType)
                {
                    case ExpressionType.Not:
                        var o = VisitExpression(expression.Operand);
                        if (!(o is string))
                        {
                            return !(bool)o;
                        }

                        var memberExpression = expression.Operand as MemberExpression;
                        if (memberExpression != null &&
                            Resolvers.Properties(memberExpression.Expression.Type).Any(p => p.Name == (string)o))
                        {
                            o = $"{o} = '1'";
                        }

                        return $"not ({o})";
                    case ExpressionType.Convert:
                        if (expression.Method != null)
                        {
                            return Expression.Lambda(expression).Compile().DynamicInvoke();
                        }
                        break;
                }

                return VisitExpression(expression.Operand);
            }

            /// <summary>
            /// Processes a new expression.
            /// </summary>
            /// <param name="expression">The new expression.</param>
            /// <returns>The result of the processing.</returns>
            protected virtual object VisitNew(NewExpression expression)
            {
                var member = Expression.Convert(expression, typeof(object));
                var lambda = Expression.Lambda<Func<object>>(member);
                var getter = lambda.Compile();
                return getter();
            }

            /// <summary>
            /// Processes a member access expression.
            /// </summary>
            /// <param name="expression">The member access expression.</param>
            /// <returns>The result of the processing.</returns>
            protected virtual object VisitMemberAccess(MemberExpression expression)
            {
                if (expression.Expression != null && expression.Expression.NodeType == ExpressionType.Parameter)
                {
                    return MemberToColumn(expression);
                }

                var member = Expression.Convert(expression, typeof(object));
                var lambda = Expression.Lambda<Func<object>>(member);
                var getter = lambda.Compile();
                return getter();
            }

            /// <summary>
            /// Processes a constant expression.
            /// </summary>
            /// <param name="expression">The constant expression.</param>
            /// <returns>The result of the processing.</returns>
            protected virtual object VisitConstantExpression(ConstantExpression expression)
            {
                return expression.Value ?? "null";
            }

            /// <summary>
            /// Proccesses a member expression.
            /// </summary>
            /// <param name="expression">The member expression.</param>
            /// <returns>The result of the processing.</returns>
            protected virtual string MemberToColumn(MemberExpression expression)
            {
                return Resolvers.Column((PropertyInfo)expression.Member);
            }

            /// <summary>
            /// Returns the expression operand for the specified expression type.
            /// </summary>
            /// <param name="expressionType">The expression type for node of an expression tree.</param>
            /// <returns>The expression operand equivalent of the <paramref name="expressionType"/>.</returns>
            protected virtual string BindOperant(ExpressionType expressionType)
            {
                switch (expressionType)
                {
                    case ExpressionType.Equal:
                        return "=";
                    case ExpressionType.NotEqual:
                        return "<>";
                    case ExpressionType.GreaterThan:
                        return ">";
                    case ExpressionType.GreaterThanOrEqual:
                        return ">=";
                    case ExpressionType.LessThan:
                        return "<";
                    case ExpressionType.LessThanOrEqual:
                        return "<=";
                    case ExpressionType.AndAlso:
                        return "and";
                    case ExpressionType.OrElse:
                        return "or";
                    case ExpressionType.Add:
                        return "+";
                    case ExpressionType.Subtract:
                        return "-";
                    case ExpressionType.Multiply:
                        return "*";
                    case ExpressionType.Divide:
                        return "/";
                    case ExpressionType.Modulo:
                        return "MOD";
                    case ExpressionType.Coalesce:
                        return "COALESCE";
                    default:
                        return expressionType.ToString();
                }
            }

            /// <summary>
            /// Returns the current SQL query.
            /// </summary>
            /// <returns>The current SQL query.</returns>
            public string ToSql()
            {
                return _whereBuilder.ToString();
            }

            /// <summary>
            /// Returns the current SQL query.
            /// </summary>
            /// <param name="parameters">When this method returns, contains the parameters for the query.</param>
            /// <returns>The current SQL query.</returns>
            public string ToSql(out DynamicParameters parameters)
            {
                parameters = _parameters;
                return _whereBuilder.ToString();
            }

            /// <summary>
            /// Returns the current SQL query.
            /// </summary>
            /// <returns>The current SQL query.</returns>
            public override string ToString()
            {
                return _whereBuilder.ToString();
            }
        }

        /// <summary>
        /// Inserts the specified entity into the database and returns the id.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="entity">The entity to be inserted.</param>
        /// <param name="transaction">Optional transaction for the command.</param>
        /// <returns>The id of the inserted entity.</returns>
        public static object Insert<TEntity>(this IDbConnection connection, TEntity entity, IDbTransaction transaction = null) where TEntity : class
        {
            var sql = BuildInsertQuery(connection, typeof(TEntity));
            return connection.ExecuteScalar(sql, entity, transaction);
        }

        /// <summary>
        /// Inserts the specified entity into the database and returns the id.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="entity">The entity to be inserted.</param>
        /// <param name="transaction">Optional transaction for the command.</param>
        /// <returns>The id of the inserted entity.</returns>
        public static Task<object> InsertAsync<TEntity>(this IDbConnection connection, TEntity entity, IDbTransaction transaction = null) where TEntity : class
        {
            var sql = BuildInsertQuery(connection, typeof(TEntity));
            return connection.ExecuteScalarAsync(sql, entity, transaction);
        }

        private static string BuildInsertQuery(IDbConnection connection, Type type)
        {
            string sql;
            if (!_insertQueryCache.TryGetValue(type, out sql))
            {
                var tableName = Resolvers.Table(type);

                bool isIdentity;
                var keyProperty = Resolvers.KeyProperty(type, out isIdentity);

                var typeProperties = new List<PropertyInfo>();
                foreach (var typeProperty in Resolvers.Properties(type))
                {
                    if (typeProperty == keyProperty)
                    {
                        if (isIdentity)
                        {
                            // Skip key properties marked as an identity column.
                            continue;
                        }
                    }

                    if (typeProperty.GetSetMethod() != null)
                    {
                        typeProperties.Add(typeProperty);
                    }
                }

                var columnNames = typeProperties.Select(Resolvers.Column).ToArray();
                var paramNames = typeProperties.Select(p => "@" + p.Name).ToArray();

                var builder = GetBuilder(connection);
                sql = builder.BuildInsert(tableName, columnNames, paramNames, keyProperty);

                _insertQueryCache.TryAdd(type, sql);
            }

            return sql;
        }

        /// <summary>
        /// Updates the values of the specified entity in the database.
        /// The return value indicates whether the operation succeeded.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="entity">The entity in the database.</param>
        /// <param name="transaction">Optional transaction for the command.</param>
        /// <returns>A value indicating whether the update operation succeeded.</returns>
        public static bool Update<TEntity>(this IDbConnection connection, TEntity entity, IDbTransaction transaction = null)
        {
            var sql = BuildUpdateQuery(typeof(TEntity));
            return connection.Execute(sql, entity, transaction) > 0;
        }

        /// <summary>
        /// Updates the values of the specified entity in the database.
        /// The return value indicates whether the operation succeeded.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="entity">The entity in the database.</param>
        /// <param name="transaction">Optional transaction for the command.</param>
        /// <returns>A value indicating whether the update operation succeeded.</returns>
        public static async Task<bool> UpdateAsync<TEntity>(this IDbConnection connection, TEntity entity, IDbTransaction transaction = null)
        {
            var sql = BuildUpdateQuery(typeof(TEntity));
            return await connection.ExecuteAsync(sql, entity, transaction) > 0;
        }

        private static string BuildUpdateQuery(Type type)
        {
            string sql;
            if (!_updateQueryCache.TryGetValue(type, out sql))
            {
                var tableName = Resolvers.Table(type);
                var keyProperty = Resolvers.KeyProperty(type);

                // Use all properties which are settable.
                var typeProperties = Resolvers.Properties(type)
                                              .Where(p => p != keyProperty)
                                              .Where(p => p.GetSetMethod() != null)
                                              .ToArray();

                var columnNames = typeProperties.Select(p => $"{Resolvers.Column(p)} = @{p.Name}").ToArray();

                sql = $"update {tableName} set {string.Join(", ", columnNames)} where {Resolvers.Column(keyProperty)} = @{keyProperty.Name}";

                _updateQueryCache.TryAdd(type, sql);
            }

            return sql;
        }

        /// <summary>
        /// Deletes the specified entity from the database.
        /// Returns a value indicating whether the operation succeeded.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="entity">The entity to be deleted.</param>
        /// <param name="transaction">Optional transaction for the command.</param>
        /// <returns>A value indicating whether the delete operation succeeded.</returns>
        public static bool Delete<TEntity>(this IDbConnection connection, TEntity entity, IDbTransaction transaction = null)
        {
            var sql = BuildDeleteQuery(typeof(TEntity));
            return connection.Execute(sql, entity, transaction) > 0;
        }

        /// <summary>
        /// Deletes the specified entity from the database.
        /// Returns a value indicating whether the operation succeeded.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="entity">The entity to be deleted.</param>
        /// <param name="transaction">Optional transaction for the command.</param>
        /// <returns>A value indicating whether the delete operation succeeded.</returns>
        public static async Task<bool> DeleteAsync<TEntity>(this IDbConnection connection, TEntity entity, IDbTransaction transaction = null)
        {
            var sql = BuildDeleteQuery(typeof(TEntity));
            return await connection.ExecuteAsync(sql, entity, transaction) > 0;
        }

        private static string BuildDeleteQuery(Type type)
        {
            string sql;
            if (!_deleteQueryCache.TryGetValue(type, out sql))
            {
                var tableName = Resolvers.Table(type);
                var keyProperty = Resolvers.KeyProperty(type);
                var keyColumnName = Resolvers.Column(keyProperty);

                sql = $"delete from {tableName} where {keyColumnName} = @{keyProperty.Name}";

                _deleteQueryCache.TryAdd(type, sql);
            }

            return sql;
        }

        /// <summary>
        /// Deletes all entities of type <typeparamref name="TEntity"/> matching the specified predicate from the database.
        /// Returns a value indicating whether the operation succeeded.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="predicate">A predicate to filter which entities are deleted.</param>
        /// <param name="transaction">Optional transaction for the command.</param>
        /// <returns>A value indicating whether the delete operation succeeded.</returns>
        public static bool DeleteMultiple<TEntity>(this IDbConnection connection, Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null)
        {
            DynamicParameters parameters;
            var sql = BuildDeleteMultipleQuery(predicate, out parameters);
            return connection.Execute(sql, parameters, transaction) > 0;
        }

        /// <summary>
        /// Deletes all entities of type <typeparamref name="TEntity"/> matching the specified predicate from the database.
        /// Returns a value indicating whether the operation succeeded.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="predicate">A predicate to filter which entities are deleted.</param>
        /// <param name="transaction">Optional transaction for the command.</param>
        /// <returns>A value indicating whether the delete operation succeeded.</returns>
        public static async Task<bool> DeleteMultipleAsync<TEntity>(this IDbConnection connection, Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null)
        {
            DynamicParameters parameters;
            var sql = BuildDeleteMultipleQuery(predicate, out parameters);
            return await connection.ExecuteAsync(sql, parameters, transaction) > 0;
        }

        private static string BuildDeleteMultipleQuery<TEntity>(Expression<Func<TEntity, bool>> predicate, out DynamicParameters parameters)
        {
            var type = typeof(TEntity);
            string sql;
            if (!_deleteAllQueryCache.TryGetValue(type, out sql))
            {
                var tableName = Resolvers.Table(type);
                sql = $"delete from {tableName}";
                _deleteAllQueryCache.TryAdd(type, sql);
            }

            sql += new SqlExpression<TEntity>()
                .Where(predicate)
                .ToSql(out parameters);
            return sql;
        }

        /// <summary>
        /// Deletes all entities of type <typeparamref name="TEntity"/> from the database.
        /// Returns a value indicating whether the operation succeeded.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="transaction">Optional transaction for the command.</param>
        /// <returns>A value indicating whether the delete operation succeeded.</returns>
        public static bool DeleteAll<TEntity>(this IDbConnection connection, IDbTransaction transaction = null)
        {
            var sql = BuildDeleteAllQuery(typeof(TEntity));
            return connection.Execute(sql, transaction: transaction) > 0;
        }

        /// <summary>
        /// Deletes all entities of type <typeparamref name="TEntity"/> from the database.
        /// Returns a value indicating whether the operation succeeded.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="connection">The connection to the database. This can either be open or closed.</param>
        /// <param name="transaction">Optional transaction for the command.</param>
        /// <returns>A value indicating whether the delete operation succeeded.</returns>
        public static async Task<bool> DeleteAllAsync<TEntity>(this IDbConnection connection, IDbTransaction transaction = null)
        {
            var sql = BuildDeleteAllQuery(typeof(TEntity));
            return await connection.ExecuteAsync(sql, transaction: transaction) > 0;
        }

        private static string BuildDeleteAllQuery(Type type)
        {
            string sql;
            if (!_deleteAllQueryCache.TryGetValue(type, out sql))
            {
                var tableName = Resolvers.Table(type);
                sql = $"delete from {tableName}";
                _deleteAllQueryCache.TryAdd(type, sql);
            }

            return sql;
        }

        /// <summary>
        /// Helper class for retrieving type metadata to build sql queries using configured resolvers.
        /// </summary>
        public static class Resolvers
        {
            private class KeyPropertyInfo
            {
                public KeyPropertyInfo(PropertyInfo propertyInfo, bool isIdentity)
                {
                    PropertyInfo = propertyInfo;
                    IsIdentity = isIdentity;
                }

                public PropertyInfo PropertyInfo { get; }

                public bool IsIdentity { get; }
            }

            private static readonly ConcurrentDictionary<Type, string> _typeTableNameCache = new ConcurrentDictionary<Type, string>();
            private static readonly ConcurrentDictionary<string, string> _columnNameCache = new ConcurrentDictionary<string, string>();
            private static readonly ConcurrentDictionary<Type, KeyPropertyInfo> _typeKeyPropertyCache = new ConcurrentDictionary<Type, KeyPropertyInfo>();
            private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _typePropertiesCache = new ConcurrentDictionary<Type, PropertyInfo[]>();
            private static readonly ConcurrentDictionary<string, ForeignKeyInfo> _typeForeignKeyPropertyCache = new ConcurrentDictionary<string, ForeignKeyInfo>();

            /// <summary>
            /// Gets the key property for the specified type, using the configured <see cref="DommelMapper.IKeyPropertyResolver"/>.
            /// </summary>
            /// <param name="type">The <see cref="System.Type"/> to get the key property for.</param>
            /// <returns>The key property for <paramref name="type"/>.</returns>
            public static PropertyInfo KeyProperty(Type type)
            {
                bool isIdentity;
                return KeyProperty(type, out isIdentity);
            }

            /// <summary>
            /// Gets the key property for the specified type, using the configured <see cref="DommelMapper.IKeyPropertyResolver"/>.
            /// </summary>
            /// <param name="type">The <see cref="Type"/> to get the key property for.</param>
            /// <param name="isIdentity">A value indicating whether the key is an identity.</param>
            /// <returns>The key property for <paramref name="type"/>.</returns>
            public static PropertyInfo KeyProperty(Type type, out bool isIdentity)
            {
                KeyPropertyInfo keyProperty;
                if (!_typeKeyPropertyCache.TryGetValue(type, out keyProperty))
                {
                    var propertyInfo = _keyPropertyResolver.ResolveKeyProperty(type, out isIdentity);
                    keyProperty = new KeyPropertyInfo(propertyInfo, isIdentity);
                    _typeKeyPropertyCache.TryAdd(type, keyProperty);
                }

                isIdentity = keyProperty.IsIdentity;
                return keyProperty.PropertyInfo;
            }

            /// <summary>
            /// Gets the foreign key property for the specified source type and including type
            /// using the configure d<see cref="IForeignKeyPropertyResolver"/>.
            /// </summary>
            /// <param name="sourceType">The source type which should contain the foreign key property.</param>
            /// <param name="includingType">The type of the foreign key relation.</param>
            /// <param name="foreignKeyRelation">The foreign key relationship type.</param>
            /// <returns>The foreign key property for <paramref name="sourceType"/> and <paramref name="includingType"/>.</returns>
            public static PropertyInfo ForeignKeyProperty(Type sourceType, Type includingType, out ForeignKeyRelation foreignKeyRelation)
            {
                var key = $"{sourceType.FullName};{includingType.FullName}";

                ForeignKeyInfo foreignKeyInfo;
                if (!_typeForeignKeyPropertyCache.TryGetValue(key, out foreignKeyInfo))
                {
                    // Resole the property and relation.
                    var foreignKeyProperty = _foreignKeyPropertyResolver.ResolveForeignKeyProperty(sourceType, includingType, out foreignKeyRelation);

                    // Cache the info.
                    foreignKeyInfo = new ForeignKeyInfo(foreignKeyProperty, foreignKeyRelation);
                    _typeForeignKeyPropertyCache.TryAdd(key, foreignKeyInfo);
                }

                foreignKeyRelation = foreignKeyInfo.Relation;
                return foreignKeyInfo.PropertyInfo;
            }

            private class ForeignKeyInfo
            {
                public ForeignKeyInfo(PropertyInfo propertyInfo, ForeignKeyRelation relation)
                {
                    PropertyInfo = propertyInfo;
                    Relation = relation;
                }

                public PropertyInfo PropertyInfo { get; private set; }

                public ForeignKeyRelation Relation { get; private set; }
            }

            /// <summary>
            /// Gets the properties to be mapped for the specified type, using the configured
            /// <see cref="DommelMapper.IPropertyResolver"/>.
            /// </summary>
            /// <param name="type">The <see cref="System.Type"/> to get the properties from.</param>
            /// <returns>>The collection of to be mapped properties of <paramref name="type"/>.</returns>
            public static IEnumerable<PropertyInfo> Properties(Type type)
            {
                PropertyInfo[] properties;
                if (!_typePropertiesCache.TryGetValue(type, out properties))
                {
                    properties = _propertyResolver.ResolveProperties(type).ToArray();
                    _typePropertiesCache.TryAdd(type, properties);
                }

                return properties;
            }

            /// <summary>
            /// Gets the name of the table in the database for the specified type,
            /// using the configured <see cref="DommelMapper.ITableNameResolver"/>.
            /// </summary>
            /// <param name="type">The <see cref="System.Type"/> to get the table name for.</param>
            /// <returns>The table name in the database for <paramref name="type"/>.</returns>
            public static string Table(Type type)
            {
                string name;
                if (!_typeTableNameCache.TryGetValue(type, out name))
                {
                    name = _tableNameResolver.ResolveTableName(type);
                    _typeTableNameCache.TryAdd(type, name);
                }
                return name;
            }

            /// <summary>
            /// Gets the name of the column in the database for the specified type,
            /// using the configured <see cref="T:DommelMapper.IColumnNameResolver"/>.
            /// </summary>
            /// <param name="propertyInfo">The <see cref="System.Reflection.PropertyInfo"/> to get the column name for.</param>
            /// <returns>The column name in the database for <paramref name="propertyInfo"/>.</returns>
            public static string Column(PropertyInfo propertyInfo)
            {
                var key = $"{propertyInfo.DeclaringType}.{propertyInfo.Name}";

                string columnName;
                if (!_columnNameCache.TryGetValue(key, out columnName))
                {
                    columnName = _columnNameResolver.ResolveColumnName(propertyInfo);
                    _columnNameCache.TryAdd(key, columnName);
                }

                return columnName;
            }

            /// <summary>
            /// Provides access to default resolver implementations.
            /// </summary>
            public static class Default
            {
                /// <summary>
                /// The default column name resolver.
                /// </summary>
                public static readonly IColumnNameResolver ColumnNameResolver = new DefaultColumnNameResolver();

                /// <summary>
                /// The default property resolver.
                /// </summary>
                public static readonly IPropertyResolver PropertyResolver = new DefaultPropertyResolver();

                /// <summary>
                /// The default key property resolver.
                /// </summary>
                public static readonly IKeyPropertyResolver KeyPropertyResolver = new DefaultKeyPropertyResolver();

                /// <summary>
                /// The default table name resolver.
                /// </summary>
                public static readonly ITableNameResolver TableNameResolver = new DefaultTableNameResolver();
            }
        }

        #region Property resolving
        private static IPropertyResolver _propertyResolver = new DefaultPropertyResolver();

        /// <summary>
        /// Defines methods for resolving the properties of entities.
        /// Custom implementations can be registerd with <see cref="M:SetPropertyResolver()"/>.
        /// </summary>
        public interface IPropertyResolver
        {
            /// <summary>
            /// Resolves the properties to be mapped for the specified type.
            /// </summary>
            /// <param name="type">The type to resolve the properties to be mapped for.</param>
            /// <returns>A collection of <see cref="PropertyInfo"/>'s of the <paramref name="type"/>.</returns>
            IEnumerable<PropertyInfo> ResolveProperties(Type type);
        }

        /// <summary>
        /// Sets the <see cref="DommelMapper.IPropertyResolver"/> implementation for resolving key of entities.
        /// </summary>
        /// <param name="propertyResolver">An instance of <see cref="DommelMapper.IPropertyResolver"/>.</param>
        public static void SetPropertyResolver(IPropertyResolver propertyResolver)
        {
            _propertyResolver = propertyResolver;
        }

        /// <summary>
        /// Represents the base class for property resolvers.
        /// </summary>
        public abstract class PropertyResolverBase : IPropertyResolver
        {
            private static readonly HashSet<Type> _primitiveTypes = new HashSet<Type>
                                                                    {
                                                                        typeof(object),
                                                                        typeof(string),
                                                                        typeof(Guid),
                                                                        typeof(decimal),
                                                                        typeof(double),
                                                                        typeof(float),
                                                                        typeof(DateTime),
                                                                        typeof(DateTimeOffset),
                                                                        typeof(TimeSpan),
                                                                        typeof(byte[])
                                                                    };

            /// <summary>
            /// Resolves the properties to be mapped for the specified type.
            /// </summary>
            /// <param name="type">The type to resolve the properties to be mapped for.</param>
            /// <returns>A collection of <see cref="PropertyInfo"/>'s of the <paramref name="type"/>.</returns>
            public abstract IEnumerable<PropertyInfo> ResolveProperties(Type type);

            /// <summary>
            /// Gets a collection of types that are considered 'primitive' for Dommel but are not for the CLR.
            /// Override this if you need your own implementation of this.
            /// </summary>
            protected virtual HashSet<Type> PrimitiveTypes => _primitiveTypes;

            /// <summary>
            /// Filters the complex types from the specified collection of properties.
            /// </summary>
            /// <param name="properties">A collection of properties.</param>
            /// <returns>The properties that are considered 'primitive' of <paramref name="properties"/>.</returns>
            protected virtual IEnumerable<PropertyInfo> FilterComplexTypes(IEnumerable<PropertyInfo> properties)
            {
                foreach (var property in properties)
                {
                    var type = property.PropertyType;
                    type = Nullable.GetUnderlyingType(type) ?? type;

                    if (type.GetTypeInfo().IsPrimitive || type.GetTypeInfo().IsEnum || PrimitiveTypes.Contains(type))
                    {
                        yield return property;
                    }
                }
            }
        }

        /// <summary>
        /// Default implemenation of the <see cref="DommelMapper.IPropertyResolver"/> interface.
        /// </summary>
        public class DefaultPropertyResolver : PropertyResolverBase
        {
            /// <inheritdoc/>
            public override IEnumerable<PropertyInfo> ResolveProperties(Type type)
            {
                return FilterComplexTypes(type.GetProperties());
            }
        }
        #endregion

        #region Key property resolving
        private static IKeyPropertyResolver _keyPropertyResolver = new DefaultKeyPropertyResolver();

        /// <summary>
        /// Sets the <see cref="DommelMapper.IKeyPropertyResolver"/> implementation for resolving key properties of entities.
        /// </summary>
        /// <param name="resolver">An instance of <see cref="DommelMapper.IKeyPropertyResolver"/>.</param>
        public static void SetKeyPropertyResolver(IKeyPropertyResolver resolver)
        {
            _keyPropertyResolver = resolver;
        }

        /// <summary>
        /// Defines methods for resolving the key property of entities.
        /// Custom implementations can be registerd with <see cref="M:SetKeyPropertyResolver()"/>.
        /// </summary>
        public interface IKeyPropertyResolver
        {
            /// <summary>
            /// Resolves the key property for the specified type.
            /// </summary>
            /// <param name="type">The type to resolve the key property for.</param>
            /// <returns>A <see cref="PropertyInfo"/> instance of the key property of <paramref name="type"/>.</returns>
            PropertyInfo ResolveKeyProperty(Type type);

            /// <summary>
            /// Resolves the key property for the specified type.
            /// </summary>
            /// <param name="type">The type to resolve the key property for.</param>
            /// <param name="isIdentity">Indicates whether the key property is an identity property.</param>
            /// <returns>A <see cref="PropertyInfo"/> instance of the key property of <paramref name="type"/>.</returns>
            PropertyInfo ResolveKeyProperty(Type type, out bool isIdentity);
        }

        /// <summary>
        /// Implements the <see cref="DommelMapper.IKeyPropertyResolver"/> interface by resolving key properties
        /// with the [Key] attribute or with the name 'Id'.
        /// </summary>
        public class DefaultKeyPropertyResolver : IKeyPropertyResolver
        {
            /// <summary>
            /// Finds the key property by looking for a property with the [Key] attribute or with the name 'Id'.
            /// </summary>
            public virtual PropertyInfo ResolveKeyProperty(Type type)
            {
                bool isIdentity;
                return ResolveKeyProperty(type, out isIdentity);
            }

            /// <summary>
            /// Finds the key property by looking for a property with the [Key] attribute or with the name 'Id'.
            /// </summary>
            public PropertyInfo ResolveKeyProperty(Type type, out bool isIdentity)
            {
                var allProps = Resolvers.Properties(type).ToList();

                // Look for properties with the [Key] attribute.
                var keyProps = allProps.Where(p => p.GetCustomAttributes(true).Any(a => a is KeyAttribute)).ToList();

                if (keyProps.Count == 0)
                {
                    // Search for properties named as 'Id' as fallback.
                    keyProps = allProps.Where(p => p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (keyProps.Count == 0)
                {
                    throw new Exception($"Could not find the key property for type '{type.FullName}'.");
                }

                if (keyProps.Count > 1)
                {
                    throw new Exception($"Multiple key properties were found for type '{type.FullName}'.");
                }

                isIdentity = true;
                return keyProps[0];
            }
        }
        #endregion

        #region Foreign key property resolving
        /// <summary>
        /// Describes a foreign key relationship.
        /// </summary>
        public enum ForeignKeyRelation
        {
            /// <summary>
            /// Specifies a one-to-one relationship.
            /// </summary>
            OneToOne,

            /// <summary>
            /// Specifies a one-to-many relationship.
            /// </summary>
            OneToMany,

            /// <summary>
            /// Specifies a many-to-many relationship.
            /// </summary>
            ManyToMany
        }

        private static IForeignKeyPropertyResolver _foreignKeyPropertyResolver = new DefaultForeignKeyPropertyResolver();

        /// <summary>
        /// Sets the <see cref="T:DommelMapper.IForeignKeyPropertyResolver"/> implementation for resolving foreign key properties.
        /// </summary>
        /// <param name="resolver">An instance of <see cref="T:DommelMapper.IForeignKeyPropertyResolver"/>.</param>
        public static void SetForeignKeyPropertyResolver(IForeignKeyPropertyResolver resolver)
        {
            _foreignKeyPropertyResolver = resolver;
        }

        /// <summary>
        /// Defines methods for resolving foreign key properties.
        /// </summary>
        public interface IForeignKeyPropertyResolver
        {
            /// <summary>
            /// Resolves the foreign key property for the specified source type and including type.
            /// </summary>
            /// <param name="sourceType">The source type which should contain the foreign key property.</param>
            /// <param name="includingType">The type of the foreign key relation.</param>
            /// <param name="foreignKeyRelation">The foreign key relationship type.</param>
            /// <returns>The foreign key property for <paramref name="sourceType"/> and <paramref name="includingType"/>.</returns>
            PropertyInfo ResolveForeignKeyProperty(Type sourceType, Type includingType, out ForeignKeyRelation foreignKeyRelation);
        }

        /// <summary>
        /// Implements the <see cref="T:DommelMapper.IForeignKeyPropertyResolver"/> interface.
        /// </summary>
        public class DefaultForeignKeyPropertyResolver : IForeignKeyPropertyResolver
        {
            /// <summary>
            /// Resolves the foreign key property for the specified source type and including type
            /// by using <paramref name="includingType"/> + Id as property name.
            /// </summary>
            /// <param name="sourceType">The source type which should contain the foreign key property.</param>
            /// <param name="includingType">The type of the foreign key relation.</param>
            /// <param name="foreignKeyRelation">The foreign key relationship type.</param>
            /// <returns>The foreign key property for <paramref name="sourceType"/> and <paramref name="includingType"/>.</returns>
            public virtual PropertyInfo ResolveForeignKeyProperty(Type sourceType, Type includingType, out ForeignKeyRelation foreignKeyRelation)
            {
                var oneToOneFk = ResolveOneToOne(sourceType, includingType);
                if (oneToOneFk != null)
                {
                    foreignKeyRelation = ForeignKeyRelation.OneToOne;
                    return oneToOneFk;
                }

                var oneToManyFk = ResolveOneToMany(sourceType, includingType);
                if (oneToManyFk != null)
                {
                    foreignKeyRelation = ForeignKeyRelation.OneToMany;
                    return oneToManyFk;
                }

                var msg = $"Could not resolve foreign key property. Source type '{sourceType.FullName}'; including type: '{includingType.FullName}'.";
                throw new Exception(msg);
            }

            private static PropertyInfo ResolveOneToOne(Type sourceType, Type includingType)
            {
                // Look for the foreign key on the source type.
                var foreignKeyName = includingType.Name + "Id";
                var foreignKeyProperty = sourceType.GetProperties().FirstOrDefault(p => p.Name == foreignKeyName);

                return foreignKeyProperty;
            }

            private static PropertyInfo ResolveOneToMany(Type sourceType, Type includingType)
            {
                // Look for the foreign key on the including type.
                var foreignKeyName = sourceType.Name + "Id";
                var foreignKeyProperty = includingType.GetProperties().FirstOrDefault(p => p.Name == foreignKeyName);
                return foreignKeyProperty;
            }
        }
        #endregion

        #region Table name resolving
        private static ITableNameResolver _tableNameResolver = new DefaultTableNameResolver();

        /// <summary>
        /// Sets the <see cref="T:Dommel.ITableNameResolver"/> implementation for resolving table names for entities.
        /// </summary>
        /// <param name="resolver">An instance of <see cref="T:Dommel.ITableNameResolver"/>.</param>
        public static void SetTableNameResolver(ITableNameResolver resolver)
        {
            _tableNameResolver = resolver;
        }

        /// <summary>
        /// Defines methods for resolving table names of entities.
        /// Custom implementations can be registerd with <see cref="M:SetTableNameResolver()"/>.
        /// </summary>
        public interface ITableNameResolver
        {
            /// <summary>
            /// Resolves the table name for the specified type.
            /// </summary>
            /// <param name="type">The type to resolve the table name for.</param>
            /// <returns>A string containing the resolved table name for for <paramref name="type"/>.</returns>
            string ResolveTableName(Type type);
        }

        /// <summary>
        /// Implements the <see cref="T:Dommel.ITableNameResolver"/> interface by resolving table names
        /// by making the type name plural and removing the 'I' prefix for interfaces.
        /// </summary>
        public class DefaultTableNameResolver : ITableNameResolver
        {
            /// <summary>
            /// Gets or sets the table to use in the database.
            /// </summary>
            public string TableName { get; protected set; }

            /// <summary>
            /// Resolves the table name by making the type plural (+ 's', Product -> Products)
            /// and removing the 'I' prefix for interfaces.
            /// </summary>
            public virtual string ResolveTableName(Type type)
            {
                var attr = ((System.Attribute[])type.GetTypeInfo().GetCustomAttributes())[0];
                var name = (attr as dynamic).TableName;
                if (type.GetTypeInfo().IsInterface && name.StartsWith("I"))
                {
                    name = name.Substring(1);
                }

                // todo: add [Table] attribute support.
                return name;
            }

            /// <summary>
            /// Creator for dynamic type
            /// </summary>
            /// <param name="tableName"></param>
            public virtual void Table(string tableName)
            {
                TableName = tableName;
            }
        }
        #endregion

        #region Column name resolving
        private static IColumnNameResolver _columnNameResolver = new DefaultColumnNameResolver();

        /// <summary>
        /// Sets the <see cref="T:Dommel.IColumnNameResolver"/> implementation for resolving column names.
        /// </summary>
        /// <param name="resolver">An instance of <see cref="T:Dommel.IColumnNameResolver"/>.</param>
        public static void SetColumnNameResolver(IColumnNameResolver resolver)
        {
            _columnNameResolver = resolver;
        }

        /// <summary>
        /// Defines methods for resolving column names for entities.
        /// Custom implementations can be registerd with <see cref="M:SetColumnNameResolver()"/>.
        /// </summary>
        public interface IColumnNameResolver
        {
            /// <summary>
            /// Resolves the column name for the specified property.
            /// </summary>
            /// <param name="propertyInfo">The property of the entity.</param>
            /// <returns>The column name for the property.</returns>
            string ResolveColumnName(PropertyInfo propertyInfo);
        }

        /// <summary>
        /// Implements the <see cref="DommelMapper.IKeyPropertyResolver"/>.
        /// </summary>
        public class DefaultColumnNameResolver : IColumnNameResolver
        {
            /// <summary>
            /// Resolves the column name for the property. This is just the name of the property.
            /// </summary>
            public virtual string ResolveColumnName(PropertyInfo propertyInfo)
            {
                return propertyInfo.Name;
            }
        }
        #endregion

        #region Sql builders
        /// <summary>
        /// Adds a custom implementation of <see cref="T:DommelMapper.ISqlBuilder"/>
        /// for the specified ADO.NET connection type.
        /// </summary>
        /// <param name="connectionType">
        /// The ADO.NET conncetion type to use the <paramref name="builder"/> with.
        /// Example: <c>typeof(SqlConnection)</c>.
        /// </param>
        /// <param name="builder">An implementation of the <see cref="T:DommelMapper.ISqlBuilder interface"/>.</param>
        public static void AddSqlBuilder(Type connectionType, ISqlBuilder builder)
        {
            _sqlBuilders[connectionType.Name.ToLower()] = builder;
        }

        private static ISqlBuilder GetBuilder(IDbConnection connection)
        {
            var connectionName = connection.GetType().Name.ToLower();
            ISqlBuilder builder;
            return _sqlBuilders.TryGetValue(connectionName, out builder) ? builder : new SqlServerSqlBuilder();
        }

        /// <summary>
        /// Defines methods for building specialized SQL queries.
        /// </summary>
        public interface ISqlBuilder
        {
            /// <summary>
            /// Builds an insert query using the specified table name, column names and parameter names.
            /// A query to fetch the new id will be included as well.
            /// </summary>
            /// <param name="tableName">The name of the table to query.</param>
            /// <param name="columnNames">The names of the columns in the table.</param>
            /// <param name="paramNames">The names of the parameters in the database command.</param>
            /// <param name="keyProperty">
            /// The key property. This can be used to query a specific column for the new id. This is
            /// optional.
            /// </param>
            /// <returns>An insert query including a query to fetch the new id.</returns>
            string BuildInsert(string tableName, string[] columnNames, string[] paramNames, PropertyInfo keyProperty);
        }

        private sealed class SqlServerSqlBuilder : ISqlBuilder
        {
            public string BuildInsert(string tableName, string[] columnNames, string[] paramNames, PropertyInfo keyProperty)
            {
                return $"set nocount on insert into {tableName} ({string.Join(", ", columnNames)}) values ({string.Join(", ", paramNames)}) select cast(scope_identity() as int)";
            }
        }

        private sealed class SqlServerCeSqlBuilder : ISqlBuilder
        {
            public string BuildInsert(string tableName, string[] columnNames, string[] paramNames, PropertyInfo keyProperty)
            {
                return $"insert into {tableName} ({string.Join(", ", columnNames)}) values ({string.Join(", ", paramNames)}) select cast(@@IDENTITY as int)";
            }
        }

        private sealed class SqliteSqlBuilder : ISqlBuilder
        {
            public string BuildInsert(string tableName, string[] columnNames, string[] paramNames, PropertyInfo keyProperty)
            {
                return $"insert into {tableName} ({string.Join(", ", columnNames)}) values ({string.Join(", ", paramNames)}); select last_insert_rowid() id";
            }
        }

        private sealed class MySqlSqlBuilder : ISqlBuilder
        {
            public string BuildInsert(string tableName, string[] columnNames, string[] paramNames, PropertyInfo keyProperty)
            {
                return $"insert into {tableName} (`{string.Join("`, `", columnNames)}`) values ({string.Join(", ", paramNames)}); select LAST_INSERT_ID() id";
            }
        }

        private sealed class PostgresSqlBuilder : ISqlBuilder
        {
            public string BuildInsert(string tableName, string[] columnNames, string[] paramNames, PropertyInfo keyProperty)
            {
                var sql = $"insert into {tableName} ({string.Join(", ", columnNames)}) values ({string.Join(", ", paramNames)})";

                if (keyProperty != null)
                {
                    var keyColumnName = Resolvers.Column(keyProperty);

                    sql += " RETURNING " + keyColumnName;
                }
                else
                {
                    // todo: what behavior is desired here?
                    throw new Exception("A key property is required for the PostgresSqlBuilder.");
                }

                return sql;
            }
        }
        #endregion
    }
}

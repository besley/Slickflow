import axios from 'axios'
import config from '../config/index.js'

const state = {
  roles: null,
  users: null
}

const getters = {
  roles: state => {
    return state.roles
  },
  users: state => {
    return state.users
  }
}

const mutations = {
  setRoles: (state, payload) => {
    state.roles = payload
  },
  setUsers: (state, payload) => {
    state.users = payload
  }
}

const actions = {
  getRoles: (context, payload) => {
    var query = {}
    query.ProcessID = config.PROCESS_A4L.ProcessID
    query.Version = config.PROCESS_A4L.Version

    console.log(config.WebApi.ApiUrl)
    axios.post(config.WebApi.ApiUrl + '/api/wf/QueryProcessRoles', query)
      .then((response) => {
        var roles = response.data.Entity
        context.commit('setRoles', roles)
      })
      .catch((error) => {
        window.console.log(error)
      })
  },
  getUsers: (context, payload) => {
    axios.get(config.WebApi.ApiUrl + '/api/wf/GetUserByRole/' + payload)
      .then((response) => {
        var users = response.data.Entity
        context.commit('setUsers', users)
      })
      .catch((error) => {
        window.console.log(error)
      })
  }
}

export default {
  namespaced: true,
  state,
  mutations,
  actions,
  getters
}

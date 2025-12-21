--
-- PostgreSQL database dump
--

\restrict 1CA3vt560wltYoE2j3fME3fVtrctNxzCPY1nGBf8oXAJ6zlESFMGWTLkwAX7m37

-- Dumped from database version 18.1
-- Dumped by pg_dump version 18.1

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: uuid-ossp; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS "uuid-ossp" WITH SCHEMA public;


--
-- Name: EXTENSION "uuid-ossp"; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION "uuid-ossp" IS 'generate universally unique identifiers (UUIDs)';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: ai_activity_config; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.ai_activity_config (
    id integer CONSTRAINT ai_axconfig_id_not_null NOT NULL,
    config_uuid character varying(50) CONSTRAINT ai_axconfig_config_uuid_not_null NOT NULL,
    process_id character varying(50) CONSTRAINT ai_axconfig_process_id_not_null NOT NULL,
    version character varying(50) CONSTRAINT ai_axconfig_version_not_null NOT NULL,
    activity_id character varying(50) CONSTRAINT ai_axconfig_activity_id_not_null NOT NULL,
    model_provider_id integer CONSTRAINT ai_axconfig_model_provider_id_not_null NOT NULL,
    model_name character varying(50) DEFAULT NULL::character varying CONSTRAINT ai_axconfig_model_name_not_null NOT NULL,
    description character varying(500) DEFAULT NULL::character varying,
    temperature numeric(3,2) DEFAULT NULL::numeric,
    max_tokens integer,
    system_prompt text,
    user_message text,
    response_format character varying(100) DEFAULT NULL::character varying,
    time_out integer,
    max_retries integer,
    error_handling character varying(100) DEFAULT NULL::character varying,
    fallback_agent character varying(255) DEFAULT NULL::character varying,
    log_level character varying(50) DEFAULT NULL::character varying,
    custom_instructions text,
    created_datetime timestamp with time zone,
    updated_datetime timestamp with time zone
);


ALTER TABLE public.ai_activity_config OWNER TO postgres;

--
-- Name: TABLE ai_activity_config; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON TABLE public.ai_activity_config IS 'AI Activity Config configuration table for storing service settings and parameters';


--
-- Name: COLUMN ai_activity_config.id; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_activity_config.id IS 'Primary key';


--
-- Name: COLUMN ai_activity_config.config_uuid; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_activity_config.config_uuid IS 'Configuration UUID identifier';


--
-- Name: COLUMN ai_activity_config.process_id; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_activity_config.process_id IS 'Process identifier';


--
-- Name: COLUMN ai_activity_config.version; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_activity_config.version IS 'Process version';


--
-- Name: COLUMN ai_activity_config.activity_id; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_activity_config.activity_id IS 'Activity id';


--
-- Name: COLUMN ai_activity_config.model_provider_id; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_activity_config.model_provider_id IS 'Model Provider Id';


--
-- Name: COLUMN ai_activity_config.model_name; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_activity_config.model_name IS 'Model Name';


--
-- Name: COLUMN ai_activity_config.description; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_activity_config.description IS 'Service description';


--
-- Name: COLUMN ai_activity_config.temperature; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_activity_config.temperature IS 'Temperature parameter for AI model';


--
-- Name: COLUMN ai_activity_config.max_tokens; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_activity_config.max_tokens IS 'Maximum tokens for AI response';


--
-- Name: COLUMN ai_activity_config.system_prompt; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_activity_config.system_prompt IS 'System prompt text';


--
-- Name: COLUMN ai_activity_config.user_message; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_activity_config.user_message IS 'Few-shot prompt examples';


--
-- Name: COLUMN ai_activity_config.response_format; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_activity_config.response_format IS 'Response format specification';


--
-- Name: COLUMN ai_activity_config.time_out; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_activity_config.time_out IS 'Timeout value in seconds';


--
-- Name: COLUMN ai_activity_config.max_retries; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_activity_config.max_retries IS 'Maximum retry attempts';


--
-- Name: COLUMN ai_activity_config.error_handling; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_activity_config.error_handling IS 'Error handling strategy';


--
-- Name: COLUMN ai_activity_config.fallback_agent; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_activity_config.fallback_agent IS 'Fallback agent identifier';


--
-- Name: COLUMN ai_activity_config.log_level; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_activity_config.log_level IS 'Logging level';


--
-- Name: COLUMN ai_activity_config.custom_instructions; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_activity_config.custom_instructions IS 'Custom instructions for the service';


--
-- Name: COLUMN ai_activity_config.created_datetime; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_activity_config.created_datetime IS 'Record creation timestamp';


--
-- Name: COLUMN ai_activity_config.updated_datetime; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_activity_config.updated_datetime IS 'Record last update timestamp';


--
-- Name: ai_agent; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.ai_agent (
    id integer NOT NULL,
    agent_name character varying NOT NULL,
    agent_identifier character varying,
    description character varying,
    category character varying,
    version character varying DEFAULT '1'::character varying NOT NULL,
    icon character varying,
    is_active smallint DEFAULT '1'::smallint,
    status character varying,
    model_provider character varying,
    model_name character varying,
    model_service character varying,
    model_version character varying,
    temperature numeric,
    max_tokens integer,
    api_endpoint character varying,
    api_key character varying,
    system_prompt text,
    few_shot_prompt text,
    response_format character varying,
    time_out integer,
    max_retries integer,
    error_handling character varying,
    fallback_agent character varying,
    log_level character varying,
    custom_instructions text,
    created_datetime timestamp with time zone,
    updated_datetime timestamp with time zone
);


ALTER TABLE public.ai_agent OWNER TO postgres;

--
-- Name: ai_agent_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.ai_agent ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.ai_agent_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: ai_agent_instance; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.ai_agent_instance (
    id integer NOT NULL,
    agent_id integer,
    version character varying
);


ALTER TABLE public.ai_agent_instance OWNER TO postgres;

--
-- Name: ai_agent_instance_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.ai_agent_instance ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.ai_agent_instance_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: ai_agent_parameter; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.ai_agent_parameter (
    id integer NOT NULL,
    agent_id integer NOT NULL,
    parameter_name character varying NOT NULL,
    direction character varying NOT NULL,
    data_type character varying NOT NULL,
    is_required smallint DEFAULT '0'::smallint,
    description text,
    default_value text,
    order_index integer DEFAULT 0
);


ALTER TABLE public.ai_agent_parameter OWNER TO postgres;

--
-- Name: ai_agent_parameter_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.ai_agent_parameter ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.ai_agent_parameter_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: ai_axconfig_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.ai_axconfig_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.ai_axconfig_id_seq OWNER TO postgres;

--
-- Name: ai_axconfig_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.ai_axconfig_id_seq OWNED BY public.ai_activity_config.id;


--
-- Name: ai_model_provider; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.ai_model_provider (
    id integer NOT NULL,
    model_provider character varying(50) NOT NULL,
    base_url character varying(500) NOT NULL,
    api_uuid character varying(50) NOT NULL,
    api_key character varying(500) NOT NULL,
    created_datetime timestamp with time zone,
    updated_datetime timestamp with time zone,
    is_active boolean DEFAULT true,
    description text
);


ALTER TABLE public.ai_model_provider OWNER TO postgres;

--
-- Name: TABLE ai_model_provider; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON TABLE public.ai_model_provider IS 'AI model provider configuration table for storing base URL and API keys';


--
-- Name: COLUMN ai_model_provider.id; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_model_provider.id IS 'Primary key';


--
-- Name: COLUMN ai_model_provider.model_provider; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_model_provider.model_provider IS 'Model provider (e.g., OpenAI, Azure OpenAI, Anthropic)';


--
-- Name: COLUMN ai_model_provider.base_url; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_model_provider.base_url IS 'Base URL for the API endpoint';


--
-- Name: COLUMN ai_model_provider.api_uuid; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_model_provider.api_uuid IS 'API uuid for generate api key';


--
-- Name: COLUMN ai_model_provider.api_key; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_model_provider.api_key IS 'API key for authentication';


--
-- Name: COLUMN ai_model_provider.created_datetime; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_model_provider.created_datetime IS 'Record creation timestamp';


--
-- Name: COLUMN ai_model_provider.updated_datetime; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_model_provider.updated_datetime IS 'Record last update timestamp';


--
-- Name: COLUMN ai_model_provider.is_active; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_model_provider.is_active IS 'Whether the model configuration is active';


--
-- Name: COLUMN ai_model_provider.description; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.ai_model_provider.description IS 'Optional description of the model configuration';


--
-- Name: ai_model_provider_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.ai_model_provider_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.ai_model_provider_id_seq OWNER TO postgres;

--
-- Name: ai_model_provider_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.ai_model_provider_id_seq OWNED BY public.ai_model_provider.id;


--
-- Name: biz_app_flow; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.biz_app_flow (
    id integer NOT NULL,
    app_name character varying NOT NULL,
    app_instance_id character varying NOT NULL,
    app_instance_code character varying,
    status character varying,
    activity_name character varying NOT NULL,
    remark character varying,
    changed_time timestamp with time zone NOT NULL,
    changed_user_id character varying NOT NULL,
    changed_user_name character varying
);


ALTER TABLE public.biz_app_flow OWNER TO postgres;

--
-- Name: biz_app_flow_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.biz_app_flow ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.biz_app_flow_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: fb_form; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.fb_form (
    id integer NOT NULL,
    form_id character varying NOT NULL,
    form_title character varying NOT NULL,
    version character varying NOT NULL,
    field_summary text,
    template_content text,
    html_content text,
    description character varying,
    created_date timestamp with time zone NOT NULL,
    updated_date timestamp with time zone
);


ALTER TABLE public.fb_form OWNER TO postgres;

--
-- Name: fb_form_data; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.fb_form_data (
    id integer NOT NULL,
    form_id integer NOT NULL,
    form_data_content character varying NOT NULL,
    created_user_id character varying,
    created_user_name character varying,
    created_date timestamp with time zone,
    updated_user_id character varying,
    updated_user_name character varying,
    updated_date timestamp with time zone,
    row_version_id bytea NOT NULL
);


ALTER TABLE public.fb_form_data OWNER TO postgres;

--
-- Name: fb_form_data_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.fb_form_data ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.fb_form_data_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: fb_form_field_activity_edit; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.fb_form_field_activity_edit (
    id integer NOT NULL,
    process_def_id integer NOT NULL,
    process_id character varying NOT NULL,
    process_name character varying,
    process_version character varying NOT NULL,
    activity_id character varying NOT NULL,
    activity_name character varying NOT NULL,
    form_id integer NOT NULL,
    form_name character varying NOT NULL,
    form_version character varying NOT NULL,
    fields_permission character varying
);


ALTER TABLE public.fb_form_field_activity_edit OWNER TO postgres;

--
-- Name: fb_form_field_activity_edit_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.fb_form_field_activity_edit ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.fb_form_field_activity_edit_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: fb_form_field_event; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.fb_form_field_event (
    id integer NOT NULL,
    form_id integer NOT NULL,
    field_id integer NOT NULL,
    event_name character varying NOT NULL,
    event_arguments character varying,
    is_disabled smallint NOT NULL,
    command_text character varying
);


ALTER TABLE public.fb_form_field_event OWNER TO postgres;

--
-- Name: fb_form_field_event_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.fb_form_field_event ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.fb_form_field_event_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: fb_form_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.fb_form ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.fb_form_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: fb_form_process; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.fb_form_process (
    id integer NOT NULL,
    form_id integer NOT NULL,
    version character varying NOT NULL,
    process_def_id integer NOT NULL,
    process_version character varying NOT NULL,
    process_id character varying NOT NULL,
    process_name character varying NOT NULL
);


ALTER TABLE public.fb_form_process OWNER TO postgres;

--
-- Name: fb_form_process_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.fb_form_process ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.fb_form_process_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: hrs_leave; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.hrs_leave (
    id integer NOT NULL,
    leave_type character varying DEFAULT '0'::character varying NOT NULL,
    days numeric NOT NULL,
    from_date date NOT NULL,
    to_date date NOT NULL,
    current_activity_text character varying,
    status integer,
    created_user_id character varying NOT NULL,
    created_user_name character varying NOT NULL,
    created_datetime timestamp with time zone NOT NULL,
    remark character varying,
    opinions character varying
);


ALTER TABLE public.hrs_leave OWNER TO postgres;

--
-- Name: hrs_leave_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.hrs_leave ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.hrs_leave_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: hrs_leave_opinion; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.hrs_leave_opinion (
    id integer NOT NULL,
    app_instance_id character varying NOT NULL,
    activity_id character varying,
    activity_name character varying NOT NULL,
    remark character varying,
    changed_time timestamp with time zone NOT NULL,
    changed_user_id integer NOT NULL,
    changed_user_name character varying
);


ALTER TABLE public.hrs_leave_opinion OWNER TO postgres;

--
-- Name: hrs_leave_opinion_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.hrs_leave_opinion ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.hrs_leave_opinion_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: man_product_order; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.man_product_order (
    id integer NOT NULL,
    order_code character varying,
    status smallint,
    product_name character varying,
    quantity integer,
    unit_price numeric,
    total_price numeric,
    created_time timestamp with time zone,
    customer_name character varying,
    address character varying,
    mobile character varying,
    remark character varying,
    updated_time timestamp with time zone
);


ALTER TABLE public.man_product_order OWNER TO postgres;

--
-- Name: man_product_order_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.man_product_order ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.man_product_order_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: sys_department; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sys_department (
    id integer NOT NULL,
    dept_code character varying NOT NULL,
    dept_name character varying NOT NULL,
    parent_dept_id integer NOT NULL,
    description character varying
);


ALTER TABLE public.sys_department OWNER TO postgres;

--
-- Name: sys_department_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.sys_department ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.sys_department_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: sys_employee; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sys_employee (
    id integer NOT NULL,
    dept_id integer NOT NULL,
    emp_code character varying NOT NULL,
    emp_name character varying NOT NULL,
    user_id integer,
    mobile character varying,
    email character varying,
    remark character varying
);


ALTER TABLE public.sys_employee OWNER TO postgres;

--
-- Name: sys_employee_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.sys_employee ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.sys_employee_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: sys_employee_manager; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sys_employee_manager (
    id integer NOT NULL,
    employee_id integer NOT NULL,
    employee_user_id integer NOT NULL,
    manager_id integer NOT NULL,
    manager_user_id integer NOT NULL
);


ALTER TABLE public.sys_employee_manager OWNER TO postgres;

--
-- Name: sys_employee_manager_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.sys_employee_manager ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.sys_employee_manager_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: sys_resource; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sys_resource (
    id integer NOT NULL,
    resource_type smallint NOT NULL,
    parent_resource_id integer NOT NULL,
    resource_name character varying NOT NULL,
    resource_code character varying NOT NULL,
    order_number smallint
);


ALTER TABLE public.sys_resource OWNER TO postgres;

--
-- Name: sys_resource_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.sys_resource ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.sys_resource_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: sys_role; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sys_role (
    id integer NOT NULL,
    role_code character varying NOT NULL,
    role_name character varying NOT NULL
);


ALTER TABLE public.sys_role OWNER TO postgres;

--
-- Name: sys_role_group_resource; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sys_role_group_resource (
    id integer NOT NULL,
    role_group_type smallint NOT NULL,
    role_group_id integer NOT NULL,
    resource_id integer NOT NULL,
    permission_type smallint NOT NULL
);


ALTER TABLE public.sys_role_group_resource OWNER TO postgres;

--
-- Name: sys_role_group_resource_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.sys_role_group_resource ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.sys_role_group_resource_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: sys_role_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.sys_role ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.sys_role_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: sys_role_user; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sys_role_user (
    id integer NOT NULL,
    role_id integer NOT NULL,
    user_id integer NOT NULL
);


ALTER TABLE public.sys_role_user OWNER TO postgres;

--
-- Name: sys_role_user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.sys_role_user ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.sys_role_user_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: sys_user; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sys_user (
    id integer NOT NULL,
    user_name character varying NOT NULL,
    email character varying
);


ALTER TABLE public.sys_user OWNER TO postgres;

--
-- Name: sys_user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.sys_user ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.sys_user_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: sys_user_resource; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sys_user_resource (
    id integer NOT NULL,
    user_id integer NOT NULL,
    resource_id integer NOT NULL
);


ALTER TABLE public.sys_user_resource OWNER TO postgres;

--
-- Name: sys_user_resource_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.sys_user_resource ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.sys_user_resource_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: tmptest; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tmptest (
    "ID" integer NOT NULL,
    "Title" character varying NOT NULL
);


ALTER TABLE public.tmptest OWNER TO postgres;

--
-- Name: wf_activity_instance; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.wf_activity_instance (
    id integer NOT NULL,
    process_instance_id integer NOT NULL,
    app_name character varying NOT NULL,
    app_instance_id character varying NOT NULL,
    app_instance_code character varying,
    process_id character varying NOT NULL,
    activity_id character varying NOT NULL,
    activity_name character varying NOT NULL,
    activity_code character varying,
    activity_type smallint NOT NULL,
    activity_state smallint DEFAULT '0'::smallint NOT NULL,
    work_item_type smallint DEFAULT '0'::smallint CONSTRAINT "wf_activity_instance_work_Item_type_not_null" NOT NULL,
    assigned_user_ids character varying,
    assigned_user_names character varying,
    backward_type smallint,
    back_src_activity_instance_id integer,
    back_org_activity_instance_id integer,
    gateway_direction_type_id smallint,
    cannot_renew_instance smallint DEFAULT '0'::smallint NOT NULL,
    approval_status smallint,
    tokens_required integer DEFAULT 1 NOT NULL,
    tokens_had integer NOT NULL,
    job_timer_type smallint,
    job_timer_status smallint,
    trigger_expression character varying,
    overdue_datetime timestamp with time zone,
    job_timer_treated_datetime timestamp with time zone,
    complex_type smallint,
    merge_type smallint,
    main_activity_instance_id integer,
    compare_type smallint,
    complete_order double precision,
    sign_forward_type smallint,
    next_step_performers character varying,
    created_user_id character varying NOT NULL,
    created_user_name character varying NOT NULL,
    created_datetime timestamp with time zone NOT NULL,
    updated_user_id character varying,
    updated_user_name character varying,
    updated_datetime timestamp with time zone,
    ended_datetime timestamp with time zone,
    ended_user_id character varying,
    ended_user_name character varying,
    record_status_invalid smallint DEFAULT '0'::smallint NOT NULL,
    row_version_id bytea
);


ALTER TABLE public.wf_activity_instance OWNER TO postgres;

--
-- Name: wf_process_instance; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.wf_process_instance (
    id integer NOT NULL,
    process_id character varying NOT NULL,
    process_name character varying NOT NULL,
    version character varying DEFAULT '1'::character varying NOT NULL,
    app_instance_id character varying NOT NULL,
    app_name character varying NOT NULL,
    app_instance_code character varying,
    process_state smallint DEFAULT '0'::smallint NOT NULL,
    sub_process_type smallint,
    sub_process_def_id integer,
    sub_process_id character varying,
    invoked_activity_instance_id integer DEFAULT 0,
    invoked_activity_id character varying,
    job_timer_type smallint,
    job_timer_status smallint,
    trigger_expression character varying,
    overdue_datetime timestamp with time zone,
    job_timer_treated_datetime timestamp with time zone,
    created_datetime timestamp with time zone NOT NULL,
    created_user_id character varying NOT NULL,
    created_user_name character varying NOT NULL,
    updated_datetime timestamp with time zone,
    updated_user_id character varying,
    updated_user_name character varying,
    ended_datetime timestamp with time zone,
    ended_user_id character varying,
    ended_user_name character varying,
    record_status_invalid smallint DEFAULT '0'::smallint NOT NULL,
    row_version_id bytea
);


ALTER TABLE public.wf_process_instance OWNER TO postgres;

--
-- Name: wf_task; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.wf_task (
    id integer NOT NULL,
    activity_instance_id integer NOT NULL,
    process_instance_id integer NOT NULL,
    app_name character varying NOT NULL,
    app_instance_id character varying NOT NULL,
    process_id character varying NOT NULL,
    activity_id character varying NOT NULL,
    activity_name character varying NOT NULL,
    task_type smallint NOT NULL,
    task_state smallint DEFAULT '0'::smallint NOT NULL,
    entrusted_task_id integer,
    assigned_user_id character varying NOT NULL,
    assigned_user_name character varying NOT NULL,
    is_email_sent smallint DEFAULT '0'::smallint NOT NULL,
    created_user_id character varying NOT NULL,
    created_user_name character varying NOT NULL,
    created_datetime timestamp with time zone NOT NULL,
    updated_datetime timestamp with time zone,
    updated_user_id character varying,
    updated_user_name character varying,
    ended_user_id character varying,
    ended_user_name character varying,
    ended_datetime timestamp with time zone,
    record_status_invalid smallint DEFAULT '0'::smallint NOT NULL,
    row_version_id bytea
);


ALTER TABLE public.wf_task OWNER TO postgres;

--
-- Name: vw_wf_task_details; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public.vw_wf_task_details AS
 SELECT t.id,
    ai.app_name,
    ai.app_instance_id,
    ai.process_id,
    pi.version,
    t.process_instance_id,
    ai.activity_id,
    t.activity_instance_id,
    ai.activity_name,
    ai.activity_code,
    ai.activity_type,
    ai.work_item_type,
    ai.back_src_activity_instance_id,
    ai.created_user_id,
    ai.created_user_name,
    t.task_type,
    t.entrusted_task_id,
    t.assigned_user_id,
    t.assigned_user_name,
    t.is_email_sent,
    t.created_datetime,
    t.updated_datetime,
    t.ended_datetime,
    t.ended_user_id,
    t.ended_user_name,
    t.task_state,
    ai.activity_state,
    t.record_status_invalid,
    pi.process_state,
    ai.complex_type,
    ai.main_activity_instance_id,
    ai.approval_status,
    ai.complete_order,
    pi.app_instance_code,
    pi.process_name,
        CASE
            WHEN (ai.main_activity_instance_id IS NULL) THEN ai.activity_state
            ELSE ( SELECT a.activity_state
               FROM public.wf_activity_instance a
              WHERE (a.id = ai.main_activity_instance_id))
        END AS main_activity_state,
    pi.sub_process_type,
    pi.sub_process_def_id,
    pi.sub_process_id
   FROM ((public.wf_activity_instance ai
     JOIN public.wf_task t ON ((ai.id = t.activity_instance_id)))
     JOIN public.wf_process_instance pi ON ((ai.process_instance_id = pi.id)));


ALTER VIEW public.vw_wf_task_details OWNER TO postgres;

--
-- Name: wf_activity_instance_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.wf_activity_instance ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.wf_activity_instance_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: wf_ai_axconfig; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.wf_ai_axconfig (
    id integer NOT NULL,
    process_id character varying DEFAULT '0'::character varying NOT NULL,
    activity_id character varying NOT NULL,
    activity_name character varying NOT NULL,
    config_uuid character varying DEFAULT '0'::character varying NOT NULL,
    model_type character varying NOT NULL,
    model_version character varying NOT NULL,
    description character varying,
    system_prompt text,
    few_shot_prompt text,
    api_key character varying NOT NULL,
    base_url character varying NOT NULL,
    temperature numeric DEFAULT 0.70,
    max_tokens integer DEFAULT 2000,
    response_format character varying,
    input_parameters jsonb,
    output_parameters jsonb,
    timeout integer DEFAULT 30,
    environment character varying DEFAULT 'production'::character varying,
    is_active text DEFAULT '0x01'::text,
    created_time timestamp with time zone,
    updated_time timestamp with time zone
);


ALTER TABLE public.wf_ai_axconfig OWNER TO postgres;

--
-- Name: wf_ai_axconfig_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.wf_ai_axconfig ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.wf_ai_axconfig_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: wf_job_info; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.wf_job_info (
    id integer NOT NULL,
    process_id character varying NOT NULL,
    process_name character varying NOT NULL,
    version character varying NOT NULL,
    activity_id character varying NOT NULL,
    activity_name character varying NOT NULL,
    activity_type character varying NOT NULL,
    trigger_type character varying NOT NULL,
    message_direction character varying NOT NULL,
    job_name character varying NOT NULL,
    topic character varying NOT NULL,
    job_status character varying NOT NULL,
    created_datetime timestamp with time zone NOT NULL,
    created_user_id character varying NOT NULL,
    created_user_name character varying NOT NULL,
    updated_datetime timestamp with time zone,
    updated_user_id character varying,
    updated_user_name character varying
);


ALTER TABLE public.wf_job_info OWNER TO postgres;

--
-- Name: wf_job_info_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.wf_job_info ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.wf_job_info_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: wf_job_log; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.wf_job_log (
    id integer NOT NULL,
    job_type character varying NOT NULL,
    job_name character varying NOT NULL,
    job_key character varying,
    ref_class character varying NOT NULL,
    ref_ids character varying NOT NULL,
    status smallint NOT NULL,
    message character varying,
    stack_trace character varying,
    inner_stack_trace character varying,
    request_data character varying,
    created_datetime timestamp with time zone NOT NULL,
    created_user_id character varying NOT NULL,
    created_user_name character varying NOT NULL
);


ALTER TABLE public.wf_job_log OWNER TO postgres;

--
-- Name: wf_job_log_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.wf_job_log ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.wf_job_log_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: wf_job_schedule; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.wf_job_schedule (
    id integer NOT NULL,
    schedule_guid character varying,
    schedule_name character varying NOT NULL,
    title character varying NOT NULL,
    schedule_type smallint NOT NULL,
    status smallint DEFAULT '0'::smallint NOT NULL,
    cron_expression character varying,
    updated_datetime timestamp with time zone,
    updated_user_id character varying,
    updated_user_name character varying
);


ALTER TABLE public.wf_job_schedule OWNER TO postgres;

--
-- Name: wf_job_schedule_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.wf_job_schedule ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.wf_job_schedule_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: wf_log; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.wf_log (
    id integer NOT NULL,
    event_type_id integer NOT NULL,
    priority integer NOT NULL,
    severity character varying NOT NULL,
    title character varying NOT NULL,
    message character varying,
    stack_trace character varying,
    inner_stack_trace character varying,
    request_data character varying,
    time_stamp timestamp with time zone NOT NULL
);


ALTER TABLE public.wf_log OWNER TO postgres;

--
-- Name: wf_log_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.wf_log ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.wf_log_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: wf_process; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.wf_process (
    id integer NOT NULL,
    process_id character varying NOT NULL,
    version character varying DEFAULT '1'::character varying NOT NULL,
    process_name character varying NOT NULL,
    process_code character varying,
    status smallint DEFAULT '0'::smallint NOT NULL,
    app_type character varying,
    package_type smallint,
    package_id integer,
    participant_guid character varying,
    page_url character varying,
    xml_file_name character varying,
    xml_file_path character varying,
    xml_content text,
    start_type smallint,
    start_expression character varying,
    end_type smallint,
    end_expression character varying,
    icon character varying,
    description character varying,
    created_datetime timestamp with time zone NOT NULL,
    updated_datetime timestamp with time zone,
    row_version_id bytea
);


ALTER TABLE public.wf_process OWNER TO postgres;

--
-- Name: wf_process_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.wf_process ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.wf_process_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: wf_process_instance_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.wf_process_instance ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.wf_process_instance_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: wf_process_variable; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.wf_process_variable (
    id integer NOT NULL,
    variable_scope character varying CONSTRAINT wf_process_variable_variable_type_not_null NOT NULL,
    app_instance_id character varying NOT NULL,
    process_id character varying NOT NULL,
    process_instance_id integer NOT NULL,
    activity_instance_id integer NOT NULL,
    activity_id character varying,
    activity_name character varying,
    name character varying NOT NULL,
    value character varying NOT NULL,
    updated_datetime timestamp with time zone CONSTRAINT wf_process_variable_last_updated_datetime_not_null NOT NULL,
    row_version_id bytea,
    media_type character varying NOT NULL
);


ALTER TABLE public.wf_process_variable OWNER TO postgres;

--
-- Name: wf_process_variable_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.wf_process_variable ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.wf_process_variable_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: wf_task_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.wf_task ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.wf_task_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: wf_transition_instance; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.wf_transition_instance (
    id integer NOT NULL,
    transition_id character varying NOT NULL,
    app_name character varying NOT NULL,
    app_instance_id character varying NOT NULL,
    process_instance_id integer NOT NULL,
    process_id character varying NOT NULL,
    transition_type smallint NOT NULL,
    flying_type smallint DEFAULT '0'::smallint NOT NULL,
    from_activity_instance_id integer NOT NULL,
    from_activity_id character varying NOT NULL,
    from_activity_type smallint NOT NULL,
    from_activity_name character varying NOT NULL,
    to_activity_instance_id integer NOT NULL,
    to_activity_id character varying NOT NULL,
    to_activity_type smallint NOT NULL,
    to_activity_name character varying NOT NULL,
    condition_parsed_result smallint DEFAULT '0'::smallint NOT NULL,
    created_user_id character varying NOT NULL,
    created_user_name character varying NOT NULL,
    created_datetime timestamp with time zone NOT NULL,
    record_status_invalid smallint DEFAULT '0'::smallint NOT NULL,
    row_version_id bytea
);


ALTER TABLE public.wf_transition_instance OWNER TO postgres;

--
-- Name: wf_transition_instance_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.wf_transition_instance ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.wf_transition_instance_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: ai_activity_config id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ai_activity_config ALTER COLUMN id SET DEFAULT nextval('public.ai_axconfig_id_seq'::regclass);


--
-- Name: ai_model_provider id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ai_model_provider ALTER COLUMN id SET DEFAULT nextval('public.ai_model_provider_id_seq'::regclass);


--
-- Name: ai_activity_config ai_activity_config_config_uuid_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ai_activity_config
    ADD CONSTRAINT ai_activity_config_config_uuid_key UNIQUE (config_uuid);


--
-- Name: ai_activity_config ai_activity_config_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ai_activity_config
    ADD CONSTRAINT ai_activity_config_pkey PRIMARY KEY (id);


--
-- Name: ai_agent_instance ai_agent_instance_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ai_agent_instance
    ADD CONSTRAINT ai_agent_instance_pkey PRIMARY KEY (id);


--
-- Name: ai_agent_parameter ai_agent_parameter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ai_agent_parameter
    ADD CONSTRAINT ai_agent_parameter_pkey PRIMARY KEY (id);


--
-- Name: ai_agent ai_agent_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ai_agent
    ADD CONSTRAINT ai_agent_pkey PRIMARY KEY (id);


--
-- Name: ai_model_provider ai_model_provider_model_provider_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ai_model_provider
    ADD CONSTRAINT ai_model_provider_model_provider_key UNIQUE (model_provider);


--
-- Name: ai_model_provider ai_model_provider_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ai_model_provider
    ADD CONSTRAINT ai_model_provider_pkey PRIMARY KEY (id);


--
-- Name: biz_app_flow biz_app_flow_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.biz_app_flow
    ADD CONSTRAINT biz_app_flow_pkey PRIMARY KEY (id);


--
-- Name: fb_form_data fb_form_data_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.fb_form_data
    ADD CONSTRAINT fb_form_data_pkey PRIMARY KEY (id);


--
-- Name: fb_form_field_activity_edit fb_form_field_activity_edit_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.fb_form_field_activity_edit
    ADD CONSTRAINT fb_form_field_activity_edit_pkey PRIMARY KEY (id);


--
-- Name: fb_form_field_event fb_form_field_event_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.fb_form_field_event
    ADD CONSTRAINT fb_form_field_event_pkey PRIMARY KEY (id);


--
-- Name: fb_form fb_form_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.fb_form
    ADD CONSTRAINT fb_form_pkey PRIMARY KEY (id);


--
-- Name: fb_form_process fb_form_process_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.fb_form_process
    ADD CONSTRAINT fb_form_process_pkey PRIMARY KEY (id);


--
-- Name: hrs_leave_opinion hrs_leave_opinion_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.hrs_leave_opinion
    ADD CONSTRAINT hrs_leave_opinion_pkey PRIMARY KEY (id);


--
-- Name: hrs_leave hrs_leave_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.hrs_leave
    ADD CONSTRAINT hrs_leave_pkey PRIMARY KEY (id);


--
-- Name: man_product_order man_product_order_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.man_product_order
    ADD CONSTRAINT man_product_order_pkey PRIMARY KEY (id);


--
-- Name: sys_department sys_department_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sys_department
    ADD CONSTRAINT sys_department_pkey PRIMARY KEY (id);


--
-- Name: sys_employee_manager sys_employee_manager_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sys_employee_manager
    ADD CONSTRAINT sys_employee_manager_pkey PRIMARY KEY (id);


--
-- Name: sys_employee sys_employee_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sys_employee
    ADD CONSTRAINT sys_employee_pkey PRIMARY KEY (id);


--
-- Name: sys_resource sys_resource_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sys_resource
    ADD CONSTRAINT sys_resource_pkey PRIMARY KEY (id);


--
-- Name: sys_role_group_resource sys_role_group_resource_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sys_role_group_resource
    ADD CONSTRAINT sys_role_group_resource_pkey PRIMARY KEY (id);


--
-- Name: sys_role sys_role_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sys_role
    ADD CONSTRAINT sys_role_pkey PRIMARY KEY (id);


--
-- Name: sys_role_user sys_role_user_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sys_role_user
    ADD CONSTRAINT sys_role_user_pkey PRIMARY KEY (id);


--
-- Name: sys_user sys_user_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sys_user
    ADD CONSTRAINT sys_user_pkey PRIMARY KEY (id);


--
-- Name: sys_user_resource sys_user_resource_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sys_user_resource
    ADD CONSTRAINT sys_user_resource_pkey PRIMARY KEY (id);


--
-- Name: wf_process_variable uq_wf_proc_var_process_activity_name; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.wf_process_variable
    ADD CONSTRAINT uq_wf_proc_var_process_activity_name UNIQUE (process_instance_id, activity_instance_id, name);


--
-- Name: wf_activity_instance wf_activity_instance_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.wf_activity_instance
    ADD CONSTRAINT wf_activity_instance_pkey PRIMARY KEY (id);


--
-- Name: wf_ai_axconfig wf_ai_axconfig_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.wf_ai_axconfig
    ADD CONSTRAINT wf_ai_axconfig_pkey PRIMARY KEY (id);


--
-- Name: wf_job_info wf_job_info_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.wf_job_info
    ADD CONSTRAINT wf_job_info_pkey PRIMARY KEY (id);


--
-- Name: wf_job_log wf_job_log_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.wf_job_log
    ADD CONSTRAINT wf_job_log_pkey PRIMARY KEY (id);


--
-- Name: wf_job_schedule wf_job_schedule_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.wf_job_schedule
    ADD CONSTRAINT wf_job_schedule_pkey PRIMARY KEY (id);


--
-- Name: wf_log wf_log_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.wf_log
    ADD CONSTRAINT wf_log_pkey PRIMARY KEY (id);


--
-- Name: wf_process_instance wf_process_instance_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.wf_process_instance
    ADD CONSTRAINT wf_process_instance_pkey PRIMARY KEY (id);


--
-- Name: wf_process wf_process_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.wf_process
    ADD CONSTRAINT wf_process_pkey PRIMARY KEY (id);


--
-- Name: wf_process_variable wf_process_variable_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.wf_process_variable
    ADD CONSTRAINT wf_process_variable_pkey PRIMARY KEY (id);


--
-- Name: wf_task wf_task_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.wf_task
    ADD CONSTRAINT wf_task_pkey PRIMARY KEY (id);


--
-- Name: wf_transition_instance wf_transition_instance_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.wf_transition_instance
    ADD CONSTRAINT wf_transition_instance_pkey PRIMARY KEY (id);


--
-- Name: idx_ai_model_provider; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_ai_model_provider ON public.ai_model_provider USING btree (model_provider);


--
-- Name: idx_wf_proc_var_process_activity; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_wf_proc_var_process_activity ON public.wf_process_variable USING btree (process_instance_id, activity_instance_id);


--
-- Name: idx_wf_proc_var_process_instance; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_wf_proc_var_process_instance ON public.wf_process_variable USING btree (process_instance_id);


--
-- Name: idx_wf_process_id_version; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX idx_wf_process_id_version ON public.wf_process USING btree (process_id, version);


--
-- Name: INDEX idx_wf_process_id_version; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON INDEX public.idx_wf_process_id_version IS 'Composite unique index on process_id and version to ensure no duplicate process versions';


--
-- PostgreSQL database dump complete
--

\unrestrict 1CA3vt560wltYoE2j3fME3fVtrctNxzCPY1nGBf8oXAJ6zlESFMGWTLkwAX7m37


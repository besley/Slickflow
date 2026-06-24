--
-- PostgreSQL database dump
--

\restrict nPhqNFJWDd0fkk9aIMsQBJTn4cuRP2cV8bW4gDX8wR9Jt1MNYwNZa9YAeiuurcZ

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

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: ai_activity_config; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.ai_activity_config (
    id integer NOT NULL,
    process_id character varying(50) NOT NULL,
    version character varying(50) NOT NULL,
    activity_id character varying(50) NOT NULL,
    model_provider_id integer NOT NULL,
    model_name character varying(50) DEFAULT NULL::character varying NOT NULL,
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
    updated_datetime timestamp with time zone,
    rag_search_strategy character varying(50) DEFAULT 'similarity'::character varying,
    rag_search_count integer DEFAULT 5,
    rag_similarity_threshold numeric(3,2) DEFAULT 0.7,
    rag_search_mode character varying(50) DEFAULT 'hybrid'::character varying,
    service_type character varying(50) DEFAULT NULL::character varying NOT NULL,
    rag_function character varying(50) DEFAULT NULL::character varying,
    memory_turns integer DEFAULT 10,
    rag_embedding_model character varying(50) DEFAULT NULL::character varying,
    rag_embedding_dimensions integer DEFAULT 1536,
    rag_embedding_model_id integer
);


--
-- Name: ai_activity_config_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.ai_activity_config_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: ai_activity_config_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.ai_activity_config_id_seq OWNED BY public.ai_activity_config.id;


--
-- Name: ai_agent; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: ai_agent_instance; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.ai_agent_instance (
    id integer NOT NULL,
    agent_id integer,
    version character varying
);


--
-- Name: ai_agent_parameter; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: ai_model_provider; Type: TABLE; Schema: public; Owner: -
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
    description text,
    model_type character varying(50) DEFAULT NULL::character varying,
    model_name character varying(100) DEFAULT NULL::character varying
);


--
-- Name: ai_model_provider_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.ai_model_provider_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: ai_model_provider_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.ai_model_provider_id_seq OWNED BY public.ai_model_provider.id;


--
-- Name: biz_app_flow; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: fb_form; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: fb_form_data; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: fb_form_field_activity_edit; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: fb_form_field_event; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: fb_form_process; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: hrs_leave; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: hrs_leave_opinion; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: man_product_order; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: sf_portal_form_data; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.sf_portal_form_data (
    app_instance_id character varying(255) NOT NULL,
    form_json text DEFAULT '{}'::text NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL
);


--
-- Name: sf_portal_quota; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.sf_portal_quota (
    tenant_id character varying(36) NOT NULL,
    period_start date DEFAULT date_trunc('month'::text, now()) NOT NULL,
    process_instances_used integer DEFAULT 0 NOT NULL,
    ai_calls_used integer DEFAULT 0 NOT NULL,
    custom_processes_used integer DEFAULT 0 NOT NULL
);


--
-- Name: sf_portal_tenant; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.sf_portal_tenant (
    id character varying(36) NOT NULL,
    email character varying(255) NOT NULL,
    display_name character varying(100),
    plan character varying(20) DEFAULT 'free'::character varying NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL
);


--
-- Name: sys_department; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.sys_department (
    id integer NOT NULL,
    dept_code character varying NOT NULL,
    dept_name character varying NOT NULL,
    parent_dept_id integer NOT NULL,
    description character varying
);


--
-- Name: sys_employee; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: sys_employee_manager; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.sys_employee_manager (
    id integer NOT NULL,
    employee_id integer NOT NULL,
    employee_user_id integer NOT NULL,
    manager_id integer NOT NULL,
    manager_user_id integer NOT NULL
);


--
-- Name: sys_resource; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.sys_resource (
    id integer NOT NULL,
    resource_type smallint NOT NULL,
    parent_resource_id integer NOT NULL,
    resource_name character varying NOT NULL,
    resource_code character varying NOT NULL,
    order_number smallint
);


--
-- Name: sys_role; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.sys_role (
    id integer NOT NULL,
    role_code character varying NOT NULL,
    role_name character varying NOT NULL
);


--
-- Name: sys_role_group_resource; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.sys_role_group_resource (
    id integer NOT NULL,
    role_group_type smallint NOT NULL,
    role_group_id integer NOT NULL,
    resource_id integer NOT NULL,
    permission_type smallint NOT NULL
);


--
-- Name: sys_role_user; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.sys_role_user (
    id integer NOT NULL,
    role_id integer NOT NULL,
    user_id integer NOT NULL
);


--
-- Name: sys_user; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.sys_user (
    id integer NOT NULL,
    user_name character varying NOT NULL,
    email character varying
);


--
-- Name: sys_user_resource; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.sys_user_resource (
    id integer NOT NULL,
    user_id integer NOT NULL,
    resource_id integer NOT NULL
);


--
-- Name: t_inquiry; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.t_inquiry (
    id uuid DEFAULT gen_random_uuid() NOT NULL,
    inquiry_no character varying(50) NOT NULL,
    company_name character varying(200) NOT NULL,
    contact_name character varying(100) NOT NULL,
    email character varying(200) NOT NULL,
    phone character varying(50),
    product_category character varying(100) NOT NULL,
    project_description text NOT NULL,
    quantity integer DEFAULT 1 NOT NULL,
    target_price numeric(18,2),
    delivery_date character varying(50),
    status smallint DEFAULT 0 NOT NULL,
    priority smallint DEFAULT 1 NOT NULL,
    estimated_value numeric(18,2) DEFAULT 0 NOT NULL,
    received_at timestamp with time zone DEFAULT now() NOT NULL,
    processed_at timestamp with time zone,
    workflow_instance_id character varying(100)
);


--
-- Name: tmptest; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tmptest (
    "ID" integer NOT NULL,
    "Title" character varying NOT NULL
);


--
-- Name: wf_activity_instance_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.wf_activity_instance_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: wf_activity_instance; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.wf_activity_instance (
    id integer DEFAULT nextval('public.wf_activity_instance_id_seq'::regclass) NOT NULL,
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
    work_item_type smallint DEFAULT '0'::smallint NOT NULL,
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


--
-- Name: wf_process_instance_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.wf_process_instance_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: wf_process_instance; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.wf_process_instance (
    id integer DEFAULT nextval('public.wf_process_instance_id_seq'::regclass) NOT NULL,
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


--
-- Name: wf_task_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.wf_task_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: wf_task; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.wf_task (
    id integer DEFAULT nextval('public.wf_task_id_seq'::regclass) NOT NULL,
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


--
-- Name: vw_wf_task_details; Type: VIEW; Schema: public; Owner: -
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


--
-- Name: wf_ai_axconfig_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.wf_ai_axconfig_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: wf_ai_axconfig; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.wf_ai_axconfig (
    id integer DEFAULT nextval('public.wf_ai_axconfig_id_seq'::regclass) NOT NULL,
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


--
-- Name: wf_job_info_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.wf_job_info_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: wf_job_info; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.wf_job_info (
    id integer DEFAULT nextval('public.wf_job_info_id_seq'::regclass) NOT NULL,
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


--
-- Name: wf_job_log_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.wf_job_log_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: wf_job_log; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.wf_job_log (
    id integer DEFAULT nextval('public.wf_job_log_id_seq'::regclass) NOT NULL,
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


--
-- Name: wf_job_schedule_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.wf_job_schedule_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: wf_job_schedule; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.wf_job_schedule (
    id integer DEFAULT nextval('public.wf_job_schedule_id_seq'::regclass) NOT NULL,
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


--
-- Name: wf_log_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.wf_log_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: wf_log; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.wf_log (
    id integer DEFAULT nextval('public.wf_log_id_seq'::regclass) NOT NULL,
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


--
-- Name: wf_process_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.wf_process_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: wf_process; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.wf_process (
    id integer DEFAULT nextval('public.wf_process_id_seq'::regclass) NOT NULL,
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


--
-- Name: wf_process_variable; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.wf_process_variable (
    id integer NOT NULL,
    variable_scope character varying NOT NULL,
    app_instance_id character varying NOT NULL,
    process_id character varying NOT NULL,
    process_instance_id integer NOT NULL,
    activity_instance_id integer NOT NULL,
    activity_id character varying,
    activity_name character varying,
    name character varying NOT NULL,
    value character varying NOT NULL,
    updated_datetime timestamp with time zone NOT NULL,
    row_version_id bytea,
    media_type character varying DEFAULT 'text'::character varying NOT NULL
);


--
-- Name: wf_process_variable_id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: wf_rule_set; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.wf_rule_set (
    id integer NOT NULL,
    rule_set_code character varying(50) NOT NULL,
    rule_set_name character varying(200) NOT NULL,
    description character varying(500) DEFAULT NULL::character varying,
    rule_content text,
    is_enabled smallint DEFAULT 1 NOT NULL,
    created_datetime timestamp with time zone DEFAULT (now() AT TIME ZONE 'utc'::text) NOT NULL,
    updated_datetime timestamp with time zone,
    mode character varying(20) DEFAULT 'ruleTypes'::character varying NOT NULL
);


--
-- Name: wf_rule_set_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.wf_rule_set_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: wf_rule_set_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.wf_rule_set_id_seq OWNED BY public.wf_rule_set.id;


--
-- Name: wf_transition_instance_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.wf_transition_instance_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: wf_transition_instance; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.wf_transition_instance (
    id integer DEFAULT nextval('public.wf_transition_instance_id_seq'::regclass) NOT NULL,
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


--
-- Name: wf_variable; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.wf_variable (
    id integer NOT NULL,
    process_id character varying(50) NOT NULL,
    version character varying(50) DEFAULT '1'::character varying NOT NULL,
    activity_id character varying(50) NOT NULL,
    name character varying(255) NOT NULL,
    type character varying(100),
    direction character varying(20) NOT NULL,
    default_value character varying(500),
    is_required smallint DEFAULT 0 NOT NULL,
    is_referenced smallint DEFAULT 0 NOT NULL,
    source_ref character varying(50),
    source_variable_name character varying(255),
    sort_order integer DEFAULT 0,
    description character varying(500),
    created_datetime timestamp with time zone DEFAULT (now() AT TIME ZONE 'utc'::text) NOT NULL,
    updated_datetime timestamp with time zone
);


--
-- Name: wf_variable_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.wf_variable_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: wf_variable_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.wf_variable_id_seq OWNED BY public.wf_variable.id;


--
-- Name: ai_activity_config id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.ai_activity_config ALTER COLUMN id SET DEFAULT nextval('public.ai_activity_config_id_seq'::regclass);


--
-- Name: ai_model_provider id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.ai_model_provider ALTER COLUMN id SET DEFAULT nextval('public.ai_model_provider_id_seq'::regclass);


--
-- Name: wf_rule_set id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.wf_rule_set ALTER COLUMN id SET DEFAULT nextval('public.wf_rule_set_id_seq'::regclass);


--
-- Name: wf_variable id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.wf_variable ALTER COLUMN id SET DEFAULT nextval('public.wf_variable_id_seq'::regclass);


--
-- Name: ai_activity_config ai_activity_config_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.ai_activity_config
    ADD CONSTRAINT ai_activity_config_pkey PRIMARY KEY (id);


--
-- Name: ai_activity_config ai_activity_config_process_id_version_activity_id_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.ai_activity_config
    ADD CONSTRAINT ai_activity_config_process_id_version_activity_id_key UNIQUE (process_id, version, activity_id);


--
-- Name: ai_agent_instance ai_agent_instance_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.ai_agent_instance
    ADD CONSTRAINT ai_agent_instance_pkey PRIMARY KEY (id);


--
-- Name: ai_agent_parameter ai_agent_parameter_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.ai_agent_parameter
    ADD CONSTRAINT ai_agent_parameter_pkey PRIMARY KEY (id);


--
-- Name: ai_agent ai_agent_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.ai_agent
    ADD CONSTRAINT ai_agent_pkey PRIMARY KEY (id);


--
-- Name: ai_model_provider ai_model_provider_model_name_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.ai_model_provider
    ADD CONSTRAINT ai_model_provider_model_name_key UNIQUE (model_name);


--
-- Name: biz_app_flow biz_app_flow_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.biz_app_flow
    ADD CONSTRAINT biz_app_flow_pkey PRIMARY KEY (id);


--
-- Name: fb_form_data fb_form_data_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.fb_form_data
    ADD CONSTRAINT fb_form_data_pkey PRIMARY KEY (id);


--
-- Name: fb_form_field_activity_edit fb_form_field_activity_edit_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.fb_form_field_activity_edit
    ADD CONSTRAINT fb_form_field_activity_edit_pkey PRIMARY KEY (id);


--
-- Name: fb_form_field_event fb_form_field_event_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.fb_form_field_event
    ADD CONSTRAINT fb_form_field_event_pkey PRIMARY KEY (id);


--
-- Name: fb_form fb_form_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.fb_form
    ADD CONSTRAINT fb_form_pkey PRIMARY KEY (id);


--
-- Name: fb_form_process fb_form_process_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.fb_form_process
    ADD CONSTRAINT fb_form_process_pkey PRIMARY KEY (id);


--
-- Name: hrs_leave_opinion hrs_leave_opinion_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.hrs_leave_opinion
    ADD CONSTRAINT hrs_leave_opinion_pkey PRIMARY KEY (id);


--
-- Name: hrs_leave hrs_leave_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.hrs_leave
    ADD CONSTRAINT hrs_leave_pkey PRIMARY KEY (id);


--
-- Name: man_product_order man_product_order_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.man_product_order
    ADD CONSTRAINT man_product_order_pkey PRIMARY KEY (id);


--
-- Name: sf_portal_form_data sf_portal_form_data_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.sf_portal_form_data
    ADD CONSTRAINT sf_portal_form_data_pkey PRIMARY KEY (app_instance_id);


--
-- Name: sf_portal_quota sf_portal_quota_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.sf_portal_quota
    ADD CONSTRAINT sf_portal_quota_pkey PRIMARY KEY (tenant_id);


--
-- Name: sf_portal_tenant sf_portal_tenant_email_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.sf_portal_tenant
    ADD CONSTRAINT sf_portal_tenant_email_key UNIQUE (email);


--
-- Name: sf_portal_tenant sf_portal_tenant_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.sf_portal_tenant
    ADD CONSTRAINT sf_portal_tenant_pkey PRIMARY KEY (id);


--
-- Name: sys_department sys_department_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.sys_department
    ADD CONSTRAINT sys_department_pkey PRIMARY KEY (id);


--
-- Name: sys_employee_manager sys_employee_manager_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.sys_employee_manager
    ADD CONSTRAINT sys_employee_manager_pkey PRIMARY KEY (id);


--
-- Name: sys_employee sys_employee_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.sys_employee
    ADD CONSTRAINT sys_employee_pkey PRIMARY KEY (id);


--
-- Name: sys_resource sys_resource_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.sys_resource
    ADD CONSTRAINT sys_resource_pkey PRIMARY KEY (id);


--
-- Name: sys_role_group_resource sys_role_group_resource_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.sys_role_group_resource
    ADD CONSTRAINT sys_role_group_resource_pkey PRIMARY KEY (id);


--
-- Name: sys_role sys_role_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.sys_role
    ADD CONSTRAINT sys_role_pkey PRIMARY KEY (id);


--
-- Name: sys_role_user sys_role_user_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.sys_role_user
    ADD CONSTRAINT sys_role_user_pkey PRIMARY KEY (id);


--
-- Name: sys_user sys_user_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.sys_user
    ADD CONSTRAINT sys_user_pkey PRIMARY KEY (id);


--
-- Name: sys_user_resource sys_user_resource_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.sys_user_resource
    ADD CONSTRAINT sys_user_resource_pkey PRIMARY KEY (id);


--
-- Name: t_inquiry t_inquiry_inquiry_no_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_inquiry
    ADD CONSTRAINT t_inquiry_inquiry_no_key UNIQUE (inquiry_no);


--
-- Name: t_inquiry t_inquiry_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_inquiry
    ADD CONSTRAINT t_inquiry_pkey PRIMARY KEY (id);


--
-- Name: wf_activity_instance wf_activity_instance_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.wf_activity_instance
    ADD CONSTRAINT wf_activity_instance_pkey PRIMARY KEY (id);


--
-- Name: wf_ai_axconfig wf_ai_axconfig_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.wf_ai_axconfig
    ADD CONSTRAINT wf_ai_axconfig_pkey PRIMARY KEY (id);


--
-- Name: wf_job_info wf_job_info_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.wf_job_info
    ADD CONSTRAINT wf_job_info_pkey PRIMARY KEY (id);


--
-- Name: wf_job_log wf_job_log_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.wf_job_log
    ADD CONSTRAINT wf_job_log_pkey PRIMARY KEY (id);


--
-- Name: wf_job_schedule wf_job_schedule_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.wf_job_schedule
    ADD CONSTRAINT wf_job_schedule_pkey PRIMARY KEY (id);


--
-- Name: wf_log wf_log_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.wf_log
    ADD CONSTRAINT wf_log_pkey PRIMARY KEY (id);


--
-- Name: wf_process_instance wf_process_instance_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.wf_process_instance
    ADD CONSTRAINT wf_process_instance_pkey PRIMARY KEY (id);


--
-- Name: wf_process wf_process_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.wf_process
    ADD CONSTRAINT wf_process_pkey PRIMARY KEY (id);


--
-- Name: wf_process wf_process_process_id_version_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.wf_process
    ADD CONSTRAINT wf_process_process_id_version_key UNIQUE (process_id, version);


--
-- Name: wf_process_variable wf_process_variable_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.wf_process_variable
    ADD CONSTRAINT wf_process_variable_pkey PRIMARY KEY (id);


--
-- Name: wf_rule_set wf_rule_set_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.wf_rule_set
    ADD CONSTRAINT wf_rule_set_pkey PRIMARY KEY (id);


--
-- Name: wf_rule_set wf_rule_set_rule_set_code_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.wf_rule_set
    ADD CONSTRAINT wf_rule_set_rule_set_code_key UNIQUE (rule_set_code);


--
-- Name: wf_task wf_task_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.wf_task
    ADD CONSTRAINT wf_task_pkey PRIMARY KEY (id);


--
-- Name: wf_transition_instance wf_transition_instance_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.wf_transition_instance
    ADD CONSTRAINT wf_transition_instance_pkey PRIMARY KEY (id);


--
-- Name: wf_variable wf_variable_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.wf_variable
    ADD CONSTRAINT wf_variable_pkey PRIMARY KEY (id);


--
-- Name: idx_t_inquiry_received; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_t_inquiry_received ON public.t_inquiry USING btree (received_at DESC);


--
-- Name: idx_t_inquiry_status; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_t_inquiry_status ON public.t_inquiry USING btree (status);


--
-- Name: idx_wf_proc_var_process_activity; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_wf_proc_var_process_activity ON public.wf_process_variable USING btree (process_instance_id, activity_instance_id);


--
-- Name: idx_wf_proc_var_process_instance; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_wf_proc_var_process_instance ON public.wf_process_variable USING btree (process_instance_id);


--
-- Name: idx_wf_rule_set_code; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_wf_rule_set_code ON public.wf_rule_set USING btree (rule_set_code);


--
-- Name: idx_wf_variable_process_activity; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_wf_variable_process_activity ON public.wf_variable USING btree (process_id, version, activity_id);


--
-- Name: uq_wf_proc_var_process_activity_name; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX uq_wf_proc_var_process_activity_name ON public.wf_process_variable USING btree (process_instance_id, activity_instance_id, name);


--
-- Name: sf_portal_quota sf_portal_quota_tenant_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.sf_portal_quota
    ADD CONSTRAINT sf_portal_quota_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES public.sf_portal_tenant(id);


--
-- PostgreSQL database dump complete
--

\unrestrict nPhqNFJWDd0fkk9aIMsQBJTn4cuRP2cV8bW4gDX8wR9Jt1MNYwNZa9YAeiuurcZ


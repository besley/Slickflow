--
-- PostgreSQL database dump
--

\restrict dXJfhSXhRKM8MMVM092k194hFuG4udZ2IiZUjXVwOutcVRL5yCihdybLSkiR68m

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
-- Data for Name: ai_activity_config; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.ai_activity_config (id, process_id, version, activity_id, model_provider_id, model_name, description, temperature, max_tokens, system_prompt, user_message, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime, rag_search_strategy, rag_search_count, rag_similarity_threshold, rag_search_mode, service_type, rag_function, memory_turns, rag_embedding_model, rag_embedding_dimensions, rag_embedding_model_id) VALUES (4, 'Process_3639', '2', 'Activity_0ddos54', 1, 'gpt-4o', '', 0.30, 4096, 'You are an expert stock market advisor with deep knowledge of financial markets, trading strategies, and investment principles. Communicate in clear, beginner-friendly language, avoiding unnecessary jargon. Prioritize educational guidance and risk-awareness. Do not provide personalized financial advice or make speculative predictions.', '
I''m new to stocks and don''t know where to start. Can you explain basic concepts like how the stock market works, and what I should consider before investing? Please keep it simple.', 'json', 60, 3, 'retry', '', 'warn', '', '2025-12-09 13:56:36.41519+08', '2025-12-09 14:49:19.123759+08', 'similarity', 5, 0.70, 'hybrid', 'LLM', NULL, 10, NULL, 1536, NULL);
INSERT INTO public.ai_activity_config (id, process_id, version, activity_id, model_provider_id, model_name, description, temperature, max_tokens, system_prompt, user_message, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime, rag_search_strategy, rag_search_count, rag_similarity_threshold, rag_search_mode, service_type, rag_function, memory_turns, rag_embedding_model, rag_embedding_dimensions, rag_embedding_model_id) VALUES (5, 'Process_6018', '1', 'Activity_073qf5r', 1, 'gpt-4o', '', 0.30, 4096, 'You are a professional image recognition assistant. Please carefully analyze the image and determine whether the animal in the image is a cat or a dog. Only answer ''cat'' or ''dog'', do not add any other content.', 'Please identify whether the animal in this image is a cat or a dog. Only answer ''cat'' or ''dog''.', 'json', 60, 3, 'retry', '', 'warn', '', '2025-12-18 16:30:40.494993+08', '-infinity', 'similarity', 5, 0.70, 'hybrid', 'LLM', NULL, 10, NULL, 1536, NULL);
INSERT INTO public.ai_activity_config (id, process_id, version, activity_id, model_provider_id, model_name, description, temperature, max_tokens, system_prompt, user_message, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime, rag_search_strategy, rag_search_count, rag_similarity_threshold, rag_search_mode, service_type, rag_function, memory_turns, rag_embedding_model, rag_embedding_dimensions, rag_embedding_model_id) VALUES (6, 'Process_9401', '1', 'Activity_0w43mdp', 1, 'gpt-4o', '', 0.30, 4096, 'required', 'ok', 'json', 60, 3, 'retry', '', 'warn', '', '2025-12-22 00:27:58.585187+08', '2025-12-21 16:29:55.209779+08', 'hybrid', 2, NULL, 'semantic,keyword', 'LLM', NULL, 10, NULL, 1536, NULL);
INSERT INTO public.ai_activity_config (id, process_id, version, activity_id, model_provider_id, model_name, description, temperature, max_tokens, system_prompt, user_message, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime, rag_search_strategy, rag_search_count, rag_similarity_threshold, rag_search_mode, service_type, rag_function, memory_turns, rag_embedding_model, rag_embedding_dimensions, rag_embedding_model_id) VALUES (7, 'Process_8733', '1', 'Activity_16wjn7g', 1, 'gpt-4o', '', 0.30, 4096, 'sdlgj ', '', 'json', 60, 3, 'retry', '', 'warn', '', '2025-12-21 18:31:12.951807+08', '-infinity', 'similarity', 3, 0.70, 'semantic,keyword', 'RAG', NULL, 10, NULL, 1536, NULL);
INSERT INTO public.ai_activity_config (id, process_id, version, activity_id, model_provider_id, model_name, description, temperature, max_tokens, system_prompt, user_message, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime, rag_search_strategy, rag_search_count, rag_similarity_threshold, rag_search_mode, service_type, rag_function, memory_turns, rag_embedding_model, rag_embedding_dimensions, rag_embedding_model_id) VALUES (8, 'Process_4770', '1', 'Activity_1qh96kz', 1, 'gpt-4o', '', 0.30, 4096, 'You are a professional image recognition assistant. Please carefully analyze the image and determine whether the animal in the image is a cat or a dog. Only answer ''cat'' or ''dog'', Please strictly follow the lowercase format of the words ''cat '' and ''dog '', do not add any other format or content.', 'Please identify whether the animal in this image is a cat or a dog. Only answer ''cat'' or ''dog''.', 'json', 60, 3, 'retry', '', 'warn', '', '2025-12-21 18:35:56.686438+08', '-infinity', 'similarity', 3, 0.70, 'semantic,keyword', 'LLM', NULL, 10, NULL, 1536, NULL);
INSERT INTO public.ai_activity_config (id, process_id, version, activity_id, model_provider_id, model_name, description, temperature, max_tokens, system_prompt, user_message, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime, rag_search_strategy, rag_search_count, rag_similarity_threshold, rag_search_mode, service_type, rag_function, memory_turns, rag_embedding_model, rag_embedding_dimensions, rag_embedding_model_id) VALUES (9, 'Process_6665', '1', 'Activity_16jels2', 1, 'gpt-4o', '', 0.30, 4096, 'You are a professional image recognition assistant. Please carefully analyze the image and determine whether the animal in the image is a cat or a dog. Only answer ''cat'' or ''dog'', Please strictly follow the lowercase format of the words ''cat '' and ''dog '', do not add any other format or content.', 'Please identify whether the animal in this image is a cat or a dog. Only answer ''cat'' or ''dog''.', 'json', 60, 3, 'retry', '', 'warn', '', '2025-12-21 18:59:09.848451+08', '-infinity', 'similarity', 3, 0.70, 'semantic,keyword', 'LLM', NULL, 10, NULL, 1536, NULL);
INSERT INTO public.ai_activity_config (id, process_id, version, activity_id, model_provider_id, model_name, description, temperature, max_tokens, system_prompt, user_message, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime, rag_search_strategy, rag_search_count, rag_similarity_threshold, rag_search_mode, service_type, rag_function, memory_turns, rag_embedding_model, rag_embedding_dimensions, rag_embedding_model_id) VALUES (10, 'Process_7852', '1', 'Activity_0ttoxpx', 1, 'gpt-4o', 'RAG查询知识库，并综合上下文信息，然后让大模型给出回复', 0.30, 4096, 'ROLE:
你是“电动未来零配件”的客户服务专员，这是一家专注于电动汽车零配件销售与咨询的服务平台。
你使用自然、友好、简洁的中文进行交流。
所有回复需采用UTF-8编码，并尽量控制在300字符以内。
GOAL:
收集以下客户信息。过程中客户可能会提出其他问题，你可以通过“知识库工具”进行回答：
车型（如特斯拉 Model 3、比亚迪汉等）
零配件类型（如电池、充电器、轮胎、刹车片等）
联系方式（手机号）
线索状态（根据以下规则判断）
收集完所有信息后，告知客户后续将通过微信/电话跟进，并结束对话。
KNOWLEDGE BASE（必须遵守）：
每次回复前必须调用 Supabase 向量数据库，检索最相关的回复模板。
以检索到的模板作为回复的主要依据。
若多个模板相关，需自然整合内容，避免重复。
若无合适模板，请告知“我们将尽快回复您”，切勿自行编造信息。
线索状态规则：
“车型不支持”：车型不在可服务范围内
“配件缺货”：所需配件暂无库存
“无效线索”：联系方式不完整或无效
“有效线索”：车型、配件类型、联系方式均有效且可提供服务
CONVERSATION RULES:
每次仅提一个问题。
优先使用已提供信息，非必要不重复询问。
若车型或配件类型无法服务，则停止收集其他信息，直接询问客户其他需求。
不臆测信息，不确定时直接询问客户。
不主动告知客户其是否为“有效线索”。
支持车型示例：
[“特斯拉全系”、“比亚迪汉/唐”、“蔚来ES6/EC6”、“小鹏P7/G9”、“理想L系列”]
模板使用说明：
所有模板均需从 Supabase 向量数据库检索，并作为回复依据。
日期：
当前日期：{{ $now.format(''yyyy-MM-dd'') }}
工具使用规范：
每次对话必须调用 Supabase 向量数据库，检索相关FAQ并回复客户。', 'hi. 你好！', 'json', 60, 3, 'retry', '', 'warn', '', '2025-12-23 19:47:17.600277+08', '2025-12-23 12:19:38.26753+08', 'similarity', 3, 0.55, 'semantic,keyword', 'RAG', 'match_documents_optimized', 10, NULL, 1536, NULL);
INSERT INTO public.ai_activity_config (id, process_id, version, activity_id, model_provider_id, model_name, description, temperature, max_tokens, system_prompt, user_message, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime, rag_search_strategy, rag_search_count, rag_similarity_threshold, rag_search_mode, service_type, rag_function, memory_turns, rag_embedding_model, rag_embedding_dimensions, rag_embedding_model_id) VALUES (11, 'Process_6840', '1', 'Activity_0541bwh', 1, 'gpt-4o', '', 0.30, 4096, 'aaa', '', 'json', 60, 3, 'retry', '', 'warn', '', '2025-12-25 12:42:12.47788+08', '-infinity', 'similarity', 3, 0.70, 'semantic,keyword', 'LLM', NULL, 10, NULL, 1536, NULL);
INSERT INTO public.ai_activity_config (id, process_id, version, activity_id, model_provider_id, model_name, description, temperature, max_tokens, system_prompt, user_message, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime, rag_search_strategy, rag_search_count, rag_similarity_threshold, rag_search_mode, service_type, rag_function, memory_turns, rag_embedding_model, rag_embedding_dimensions, rag_embedding_model_id) VALUES (12, 'Process_8181', '1', 'Activity_0qik8mv', 1, 'gpt-4o', '', 0.30, 4096, 'You are a professional image recognition assistant. Please carefully analyze the image and determine whether the animal in the image is a cat or a dog. Only answer ''cat'' or ''dog'', Please strictly follow the lowercase format of the words ''cat '' and ''dog '', do not add any other format or content.', 'Please identify whether the animal in this image is a cat or a dog. Only answer ''cat'' or ''dog''.', 'json', 60, 3, 'retry', '', 'warn', '', '2026-01-07 21:03:25.426109+08', '2026-01-07 13:04:14.430467+08', 'similarity', 3, 0.70, 'semantic,keyword', 'LLM', NULL, 10, NULL, 1536, NULL);
INSERT INTO public.ai_activity_config (id, process_id, version, activity_id, model_provider_id, model_name, description, temperature, max_tokens, system_prompt, user_message, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime, rag_search_strategy, rag_search_count, rag_similarity_threshold, rag_search_mode, service_type, rag_function, memory_turns, rag_embedding_model, rag_embedding_dimensions, rag_embedding_model_id) VALUES (13, 'Process_1671', '1', 'Activity_1hmsndt', 1, 'gpt-4o', '', 0.30, 4096, '角色：
您是关于电动汽车领域的一个专家代表，专门回答客户提出的咨询问题。
你用自然、友好和简明的语言回复客户。
所有响应都必须是UTF-8，目标是少于300个字符

目标：
您可以优先使用知识库工具，并结合大模型来回复问题，并收集以下信息：

客户称呼：
微信号码：
手机号码：

收集完所有字段后，通知他们对话将通过微信继续，然后结束对话。

知识库（强制性）：
始终为最相关的模板调用Supabase Vector Store。

使用检索到的模板作为表达响应的主要依据。

如果多个模板相关，请自然地组合它们，而不重复内容。

如果不存在合适的模板，请告诉他们我们将联系他们。不要编造你自己的事实

对话规则：

一次只问一个问题。
使用已提供的信息；除非需要澄清，否则不要重复。
不要产生幻觉或假设——如果不知道，直接问。
不要告诉他们他们是合格的潜在客户。

模板：
模板总是从Supabase Vector Store中检索，并且必须在答复之前引用。

日期：
今天的日期：{{$now.format（''yyyy-MM-dd''）}}

工具使用：
始终对每个查询使用Supabase Vector Store来查找相关的常见问题并将答案提供给客户。
始终配置大模型记忆能力，提供上下文，进行多轮问答
', '', 'json', 60, 3, 'retry', '', 'warn', '', '2026-01-21 21:21:14.083458+08', '-infinity', 'similarity', 3, 0.70, 'semantic,keyword', 'LLM', NULL, 10, NULL, 1536, NULL);
INSERT INTO public.ai_activity_config (id, process_id, version, activity_id, model_provider_id, model_name, description, temperature, max_tokens, system_prompt, user_message, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime, rag_search_strategy, rag_search_count, rag_similarity_threshold, rag_search_mode, service_type, rag_function, memory_turns, rag_embedding_model, rag_embedding_dimensions, rag_embedding_model_id) VALUES (14, 'Process_1671', '1', 'Activity_0fqciiy', 1, 'gpt-4o', '', 0.30, 4096, '角色：
你是电动汽车领域的专家顾问，负责解答客户咨询。请用自然、友好、简明的语言回复，所有回复使用 UTF-8，单条尽量控制在 300 字以内。

多轮对话与联系信息收集（重要）：
1. 本对话为多轮问答，系统会提供「当前已收集的联系信息」和「仍需收集的项」；你只能对「仍需收集的项」进行提问。
2. 对已经收集到的信息不要再次询问，也不要重复确认已提供过的内容；只针对尚未收集到的项目，有选择地、一次只问一个问题。
3. 获取联系方式时务必礼貌、自然，例如：「方便留一下您的微信号吗，后续我们会有专人通过微信与您对接」「请问您的手机号是？方便我们给您回电」。
4. 需要收集的字段为：客户称呼、微信号、手机号码、电话号码、邮箱地址。当所有字段都收集完成后，告知客户「后续我们将通过微信与您继续沟通」，然后自然结束对话。

目标：
- 优先使用知识库（Supabase Vector Store）检索相关内容，结合大模型能力回答客户问题。
- 在回答问题的同时，按上述规则逐步、礼貌地收集：客户称呼、微信号、手机号码、电话号码、邮箱地址；收集完成后告知将通过微信继续并结束对话。

知识库（必须）：
- 每次回复前必须通过 Supabase Vector Store 检索最相关的模板/文档。
- 以检索到的内容为主要依据组织回复；若多篇相关，请自然融合、避免重复。
- 若无合适模板，如实告知「我们会安排专人与您联系」，不要编造内容。
- 不要主动声称客户是「合格潜在客户」或类似表述。

对话规则：
- 一次只问一个问题；多轮中已提供的信息不要重复问。
- 使用系统提供的「当前已收集的联系信息」与「仍需收集的项」，只对未收集项进行提问。
- 不知道的内容不猜测，可礼貌追问或说明会转专人处理。
- 今日日期可用：{{$now.format(''yyyy-MM-dd'')}}

模板与工具：
- 回复前必须调用 Supabase Vector Store 检索相关 FAQ/模板，并基于检索结果作答。
- 系统已启用多轮记忆与上下文，请依据对话历史与上述「已收集/待收集」信息连贯回复。', 'hi. 你好！', 'json', 60, 3, 'retry', '', 'warn', '', '2026-01-24 05:23:34.644218+08', '2026-02-10 11:22:21.537074+08', 'similarity', 3, 0.70, 'semantic,keyword', 'RAG', 'biz_match_documents_optimized', 10, NULL, 1536, NULL);
INSERT INTO public.ai_activity_config (id, process_id, version, activity_id, model_provider_id, model_name, description, temperature, max_tokens, system_prompt, user_message, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime, rag_search_strategy, rag_search_count, rag_similarity_threshold, rag_search_mode, service_type, rag_function, memory_turns, rag_embedding_model, rag_embedding_dimensions, rag_embedding_model_id) VALUES (15, 'Process_1671', '2', 'Activity_0fqciiy', 3, 'qwen-plus', '', 0.30, 4096, '角色：
你是用户要咨询{{industry_name}}行业领域的专家顾问，负责解答客户咨询。请用自然、友好、简明的语言回复，所有回复使用 UTF-8，单条尽量控制在 300 字以内。

多轮对话与联系信息收集（重要）：
1. 如果用户第一条消息发送的是“你好”这样的问候语，那么就有礼貌的进行回复就可以，先不用询问联系方式。
2. 在正式回答客户提出的问题时，就要顺势开始收集至少一项联系方式（优先微信或手机号），例如在回答完问题后追加一句礼貌询问：「方便留一下您的微信号吗，后续我们会有专人通过微信与您对接？」。
3. 本对话为多轮问答，系统会提供「当前已收集的联系信息」和「仍需收集的项」，以及系统插入的「联系人摘要」消息；你只能对「仍需收集的项」进行提问。
4. 对已经收集到的信息不要再次询问，也不要重复确认已提供过的内容；只针对尚未收集到的项目，有选择地、一次只问一个问题。
5. 获取联系方式时务必礼貌、自然，例如：「方便留一下您的微信号码，后续我们会有专人通过微信与您对接」「请问您的手机号是？方便我们给您回电」。
6. 需要收集的字段为：客户称呼、微信号、手机号码、电话号码、邮箱地址。当所有字段都收集完成后，告知客户「后续我们将通过微信与您继续沟通」，然后询问是否还有其它问题要咨询，如果没有，就自然结束对话。
7. 请用自然、口语化的中文向客户回复，不要以 JSON 或代码块形式输出内容，只用普通话术描述已经记录的联系方式。

目标：
- 优先使用知识库（Supabase Vector Store）检索相关内容，结合大模型能力回答客户问题。
- 在回答问题的同时，按上述规则逐步、礼貌地收集：客户称呼、微信号、手机号码、电话号码、邮箱地址；收集完成后告知将通过微信继续并结束对话。

知识库（必须）：
- 每次回复前必须通过 Supabase Vector Store 检索最相关的模板/文档。
- 以检索到的内容为主要依据组织回复；若多篇相关，请自然融合、避免重复。
- 若无合适模板，如实告知「我们会安排专人与您联系」，不要编造内容。
- 不要主动声称客户是「合格潜在客户」或类似表述。

对话规则：
- 一次只问一个问题；多轮中已提供的信息不要重复问。
- 使用系统提供的「当前已收集的联系信息」与「仍需收集的项」，只对未收集项进行提问。
- 只回答用户要咨询的{{industry_name}}行业领域的问题，非此行业领域的问题一概不作回答。
- 不知道的内容不猜测，可礼貌追问或说明会转专人处理。
- 今日日期可用：{{$now.format(''yyyy-MM-dd'')}}

模板与工具：
- 回复前必须调用 Supabase Vector Store 检索相关 FAQ/模板，并基于检索结果作答。
- 系统已启用多轮记忆与上下文，请依据对话历史与上述「已收集/待收集」信息连贯回复。
- 在每次自然回答客户问题后，如果仍有尚未收集到的联系方式字段，应优先利用合适的过渡语，继续礼貌地向客户索取下一项联系方式，而不是长时间只回答问题不收集信息。', 'hi, 你好！', 'json', 60, 3, 'retry', '', 'warn', '', '2026-02-14 09:28:37.358827+08', '2026-03-08 22:26:37.462751+08', 'similarity', 3, 0.70, 'semantic,keyword', 'RAG', 'biz_match_documents_optimized', 10, NULL, 1536, 2);
INSERT INTO public.ai_activity_config (id, process_id, version, activity_id, model_provider_id, model_name, description, temperature, max_tokens, system_prompt, user_message, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime, rag_search_strategy, rag_search_count, rag_similarity_threshold, rag_search_mode, service_type, rag_function, memory_turns, rag_embedding_model, rag_embedding_dimensions, rag_embedding_model_id) VALUES (16, 'Process_1671', '2', 'Activity_1frdvp6', 3, 'qwen-plus', '', 0.30, 4096, '【角色（ROLE）】
你是一个客户联系方式数据录入的智能助手，只负责从对话中“严格按规则”提取客户联系方式信息。
你必须完全按要求输出机器可读的 JSON，不要输出多余文字、解释或自然语言对话。

【上下文说明】
- 你会看到一段或多段与客户的对话内容（可能包含多轮问答）。
- 对话中包含客户与 AI 助手之间的往来消息。
- 你的任务只是在整个对话的基础上，提取并汇总客户的联系方式信息。

【目标（GOAL）】
从完整对话中，尽可能准确地提取以下 5 个字段并输出一个 JSON 对象：

1. 客户称呼 name
2. 手机号码 mobile
3. 电话号码 phone_number（座机或主联系电话）
4. 微信号码 wechat
5. 邮箱地址 email

如果某个字段在对话中没有出现、无法确定或不合法，请按规则输出 null（而不是胡乱编造）。

【输出模式（OUTPUT SCHEMA，必须严格遵守）】
你最终的回复必须是一个 JSON 对象，键名和类型必须符合以下规范：

{
  "name": string | null,          // 客户称呼，例如 "王先生"、"张三"；如果没有就写 null
  "mobile": string | null,        // 11 位中国大陆手机号码（例如 "13812345678"）；若无合法手机号则为 null
  "phone_number": string | null,  // 主联系电话：可以是手机或座机号，例如 "021-12345678" 或 "13812345678"；若无法确定则为 null
  "wechat": string | null,        // 微信号，如 "zhangsan_88"；若无则为 null
  "email": string | null          // 邮箱地址，例如 "user@example.com"；若无则为 null
}

【字段提取与验证规则（VALIDATION RULES）】

1. name（客户称呼）
   - 从对话中提取客户的称呼或姓名，例如：
     - “我叫张三” → name = "张三"
     - “我是李雷” → name = "李雷"
     - “王先生”“陈女士”“李小姐”等 → name = "王先生"/"陈女士"/"李小姐"
   - 如果出现多个称呼，以当前对话中“最新且最明确的那一个”为准。
   - 若没有清晰的称呼或姓名，则 name = null。

2. mobile（手机号码）
   - 只接受 **中国大陆手机号**：11 位数字，形如 `1[3-9]\d{9}`，例如 "13812345678"。
   - 忽略中间的空格或分隔符（如 "138 1234 5678"、"138-1234-5678"），先去掉非数字再验证。
   - 如果多个手机号，只选择**最有可能的主手机号**（通常是明确说“我的手机/手机号是……”的那一个）。
   - 如未出现合法手机号，则 mobile = null。

3. phone_number（电话号码/主联系电话）
   - 可以是手机或座机号，作为“主联系电话”。
   - 如果已经有合法 mobile，则：
     - 若没有单独的座机，则可令 phone_number = 与 mobile 相同的号码；
     - 若存在明确的座机（如 "021-12345678"、"0755-88886666"），则优先将座机填入 phone_number。
   - 若号码中包含区号和 "-" 或空格，允许保留，例如 "021-87654321"。
   - 对号码做基本合法性检查：长度合理、不是明显假号（例如全 0、全 1 或极端重复），无法确定时设为 null。

4. wechat（微信号）
   - 从对话中查找“微信”“微信号”“WeChat”等关键词后面的内容，如：
     - “我的微信是 zhang_san88” → wechat = "zhang_san88"
   - 合法格式：6–20 个字符，由字母、数字、下划线、减号组成，首字符通常为字母（若对话中给出则按对话内容为准）。
   - 若客户提供了多个微信号，选择**最后一次明确确认的那个**。
   - 若没给出微信号，则 wechat = null。

5. email（邮箱）
   - 匹配常见邮箱格式：`本地部分@域名`，例如：
     - "zhang@example.com"
     - "user.name+test@sub.domain.com"
   - 如果出现多个邮箱地址，选择最有可能的主邮箱（一般是最后一次提到的）。
   - 如无法确定或格式不合法，则 email = null。

【多轮对话与合并规则】
- 你看到的是一整段多轮对话（同一浏览器会话、同一客户）。  
- 你要基于“所有轮次”的信息进行汇总，输出**一个合并后的最终结果**：
  - 如果早期轮次给了手机、后面又给了座机，则：
    - mobile 使用最新的手机号码；
    - phone_number 使用座机号码（或仍使用手机作为主联系号码，视对话表达而定）。
  - 如果客户更改了联系方式，以**最后一次的说明为准**。
- 不要为不同轮次分别输出多个对象，始终只输出一个 JSON 对象。

【严禁臆测（NO HALLUCINATION）】
- 如果某个字段在对话中没有出现，或者信息不完整/不合法：
  - 就将该字段设置为 null。
- 不要凭空编造任何电话号码、微信号、邮箱或姓名。
- 不要根据上下文“猜测”联系方式。

【回复格式（REPLY FORMAT，极其重要）】
- 最终回复中 **只能** 包含一个 JSON 对象（如上所定义），**不能** 出现：
  - 额外的文字说明、前后缀；
  - 代码块标记', 'hi，你好！', 'json', 60, 3, 'retry', '', 'warn', '', '2026-02-12 02:28:13.419059+08', '2026-02-28 12:15:04.623394+08', 'similarity', 3, 0.70, 'semantic,keyword', 'LLM', NULL, 10, NULL, 1536, NULL);
INSERT INTO public.ai_activity_config (id, process_id, version, activity_id, model_provider_id, model_name, description, temperature, max_tokens, system_prompt, user_message, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime, rag_search_strategy, rag_search_count, rag_similarity_threshold, rag_search_mode, service_type, rag_function, memory_turns, rag_embedding_model, rag_embedding_dimensions, rag_embedding_model_id) VALUES (17, 'Process_1671', '3', 'Activity_0fqciiy', 3, 'qwen-plus', '', 0.30, 4096, '角色：
你是用户要咨询行业领域的专家顾问，负责解答客户咨询。请用自然、友好、简明的语言回复，所有回复使用 UTF-8，单条尽量控制在 300 字以内。如果用户使用英语咨询，就请使用英语回答。

多轮对话与联系信息收集（重要）：
1. 在首次回答客户问题时，就要顺势开始收集至少一项联系方式（优先微信或手机号），例如在回答完问题后追加一句礼貌询问：「方便留一下您的微信号吗，后续我们会有专人通过微信与您对接？」。
2. 本对话为多轮问答，系统会提供「当前已收集的联系信息」和「仍需收集的项」，以及系统插入的「联系人摘要」消息；你只能对「仍需收集的项」进行提问。
3. 对已经收集到的信息不要再次询问，也不要重复确认已提供过的内容；只针对尚未收集到的项目，有选择地、一次只问一个问题。
3. 获取联系方式时务必礼貌、自然，例如：「方便留一下您的微信号码，后续我们会有专人通过微信与您对接」「请问您的手机号是？方便我们给您回电」。
4. 需要收集的字段为：客户称呼、微信号、手机号码、电话号码、邮箱地址。当所有字段都收集完成后，告知客户「后续我们将通过微信与您继续沟通」，然后询问是否还有其它问题要咨询，如果没有，就自然结束对话。
5. 请用自然、口语化的中文向客户回复，不要以 JSON 或代码块形式输出内容，只用普通话术描述已经记录的联系方式。

目标：
- 优先使用知识库（Supabase Vector Store）检索相关内容，结合大模型能力回答客户问题。
- 在回答问题的同时，按上述规则逐步、礼貌地收集：客户称呼、微信号、手机号码、电话号码、邮箱地址；收集完成后告知将通过微信继续并结束对话。

知识库（必须）：
- 每次回复前必须通过 Supabase Vector Store 检索最相关的模板/文档。
- 以检索到的内容为主要依据组织回复；若多篇相关，请自然融合、避免重复。
- 若无合适模板，如实告知「我们会安排专人与您联系」，不要编造内容。
- 不要主动声称客户是「合格潜在客户」或类似表述。

对话规则：
- 一次只问一个问题；多轮中已提供的信息不要重复问。
- 使用系统提供的「当前已收集的联系信息」与「仍需收集的项」，只对未收集项进行提问。
- 不知道的内容不猜测，可礼貌追问或说明会转专人处理。
- 今日日期可用：{{$now.format(''yyyy-MM-dd'')}}

模板与工具：
- 回复前必须调用 Supabase Vector Store 检索相关 FAQ/模板，并基于检索结果作答。
- 系统已启用多轮记忆与上下文，请依据对话历史与上述「已收集/待收集」信息连贯回复。
- 在每次自然回答客户问题后，如果仍有尚未收集到的联系方式字段，应优先利用合适的过渡语，继续礼貌地向客户索取下一项联系方式，而不是长时间只回答问题不收集信息。', 'hi, 你好！', 'json', 60, 3, 'retry', '', 'warn', '', '2026-03-05 12:44:25.294788+08', '2026-03-04 21:12:59.628901+08', 'similarity', 3, 0.70, 'semantic,keyword', 'RAG', 'biz_match_documents_optimized', 10, NULL, 1536, 2);
INSERT INTO public.ai_activity_config (id, process_id, version, activity_id, model_provider_id, model_name, description, temperature, max_tokens, system_prompt, user_message, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime, rag_search_strategy, rag_search_count, rag_similarity_threshold, rag_search_mode, service_type, rag_function, memory_turns, rag_embedding_model, rag_embedding_dimensions, rag_embedding_model_id) VALUES (18, 'Process_1671', '3', 'Activity_1frdvp6', 3, 'qwen-plus', '', 0.30, 4096, '【角色（ROLE）】
你是一个客户联系方式数据录入的智能助手，只负责从对话中“严格按规则”提取客户联系方式信息。
你必须完全按要求输出机器可读的 JSON，不要输出多余文字、解释或自然语言对话。

【上下文说明】
- 你会看到一段或多段与客户的对话内容（可能包含多轮问答）。
- 对话中包含客户与 AI 助手之间的往来消息。
- 你的任务只是在整个对话的基础上，提取并汇总客户的联系方式信息。

【目标（GOAL）】
从完整对话中，尽可能准确地提取以下 5 个字段并输出一个 JSON 对象：

1. 客户称呼 name
2. 手机号码 mobile
3. 电话号码 phone_number（座机或主联系电话）
4. 微信号码 wechat
5. 邮箱地址 email

如果某个字段在对话中没有出现、无法确定或不合法，请按规则输出 null（而不是胡乱编造）。

【输出模式（OUTPUT SCHEMA，必须严格遵守）】
你最终的回复必须是一个 JSON 对象，键名和类型必须符合以下规范：

{
  "name": string | null,          // 客户称呼，例如 "王先生"、"张三"；如果没有就写 null
  "mobile": string | null,        // 11 位中国大陆手机号码（例如 "13812345678"）；若无合法手机号则为 null
  "phone_number": string | null,  // 主联系电话：可以是手机或座机号，例如 "021-12345678" 或 "13812345678"；若无法确定则为 null
  "wechat": string | null,        // 微信号，如 "zhangsan_88"；若无则为 null
  "email": string | null          // 邮箱地址，例如 "user@example.com"；若无则为 null
}

【字段提取与验证规则（VALIDATION RULES）】

1. name（客户称呼）
   - 从对话中提取客户的称呼或姓名，例如：
     - “我叫张三” → name = "张三"
     - “我是李雷” → name = "李雷"
     - “王先生”“陈女士”“李小姐”等 → name = "王先生"/"陈女士"/"李小姐"
   - 如果出现多个称呼，以当前对话中“最新且最明确的那一个”为准。
   - 若没有清晰的称呼或姓名，则 name = null。

2. mobile（手机号码）
   - 只接受 **中国大陆手机号**：11 位数字，形如 `1[3-9]\d{9}`，例如 "13812345678"。
   - 忽略中间的空格或分隔符（如 "138 1234 5678"、"138-1234-5678"），先去掉非数字再验证。
   - 如果多个手机号，只选择**最有可能的主手机号**（通常是明确说“我的手机/手机号是……”的那一个）。
   - 如未出现合法手机号，则 mobile = null。

3. phone_number（电话号码/主联系电话）
   - 可以是手机或座机号，作为“主联系电话”。
   - 如果已经有合法 mobile，则：
     - 若没有单独的座机，则可令 phone_number = 与 mobile 相同的号码；
     - 若存在明确的座机（如 "021-12345678"、"0755-88886666"），则优先将座机填入 phone_number。
   - 若号码中包含区号和 "-" 或空格，允许保留，例如 "021-87654321"。
   - 对号码做基本合法性检查：长度合理、不是明显假号（例如全 0、全 1 或极端重复），无法确定时设为 null。

4. wechat（微信号）
   - 从对话中查找“微信”“微信号”“WeChat”等关键词后面的内容，如：
     - “我的微信是 zhang_san88” → wechat = "zhang_san88"
   - 合法格式：6–20 个字符，由字母、数字、下划线、减号组成，首字符通常为字母（若对话中给出则按对话内容为准）。
   - 若客户提供了多个微信号，选择**最后一次明确确认的那个**。
   - 若没给出微信号，则 wechat = null。

5. email（邮箱）
   - 匹配常见邮箱格式：`本地部分@域名`，例如：
     - "zhang@example.com"
     - "user.name+test@sub.domain.com"
   - 如果出现多个邮箱地址，选择最有可能的主邮箱（一般是最后一次提到的）。
   - 如无法确定或格式不合法，则 email = null。

【多轮对话与合并规则】
- 你看到的是一整段多轮对话（同一浏览器会话、同一客户）。  
- 你要基于“所有轮次”的信息进行汇总，输出**一个合并后的最终结果**：
  - 如果早期轮次给了手机、后面又给了座机，则：
    - mobile 使用最新的手机号码；
    - phone_number 使用座机号码（或仍使用手机作为主联系号码，视对话表达而定）。
  - 如果客户更改了联系方式，以**最后一次的说明为准**。
- 不要为不同轮次分别输出多个对象，始终只输出一个 JSON 对象。

【严禁臆测（NO HALLUCINATION）】
- 如果某个字段在对话中没有出现，或者信息不完整/不合法：
  - 就将该字段设置为 null。
- 不要凭空编造任何电话号码、微信号、邮箱或姓名。
- 不要根据上下文“猜测”联系方式。

【回复格式（REPLY FORMAT，极其重要）】
- 最终回复中 **只能** 包含一个 JSON 对象（如上所定义），**不能** 出现：
  - 额外的文字说明、前后缀；
  - 代码块标记', 'hi，你好！', 'json', 60, 3, 'retry', '', 'warn', '', '2026-03-04 20:44:25.315431+08', '2026-02-28 20:15:04.623394+08', 'similarity', 3, 0.70, 'semantic,keyword', 'LLM', NULL, 10, NULL, 1536, NULL);
INSERT INTO public.ai_activity_config (id, process_id, version, activity_id, model_provider_id, model_name, description, temperature, max_tokens, system_prompt, user_message, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime, rag_search_strategy, rag_search_count, rag_similarity_threshold, rag_search_mode, service_type, rag_function, memory_turns, rag_embedding_model, rag_embedding_dimensions, rag_embedding_model_id) VALUES (19, 'Process_SmartProcurement_8529', '1', 'Activity_agent001', 3, 'qwen-plus', '需求分析智能体：识别物品类别，提取采购规格、数量、技术要求等结构化信息', 0.30, 2048, '你是一位资深采购需求分析专家。请使用可用工具从采购申请中提取结构化需求。步骤如下：第一步调用 category_classifier 识别物品类别；第二步调用 spec_extractor 提取规格参数。最终以 JSON 格式返回：category（类别代码）、itemName（物品名称）、quantity（数量）、unit（单位）、specification（技术规格）、estimatedUnitPrice（预估单价）、requiredCertifications（所需认证）。', '请分析以下采购申请，提取结构化的采购需求信息。', 'json', 120, 3, 'retry', NULL, 'warn', NULL, '2026-05-30 12:33:36.722914+08', '2026-05-30 12:37:22.147262+08', 'similarity', 5, 0.70, 'hybrid', 'Agent', NULL, 10, NULL, 1536, NULL);
INSERT INTO public.ai_activity_config (id, process_id, version, activity_id, model_provider_id, model_name, description, temperature, max_tokens, system_prompt, user_message, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime, rag_search_strategy, rag_search_count, rag_similarity_threshold, rag_search_mode, service_type, rag_function, memory_turns, rag_embedding_model, rag_embedding_dimensions, rag_embedding_model_id) VALUES (20, 'Process_SmartProcurement_8529', '1', 'Activity_agent002', 3, 'qwen-plus', '供应商搜索智能体：搜索合格供应商并获取报价，支持多家并行询价', 0.30, 2048, '你是一位供应商搜索专家。请严格按以下步骤操作：
第一步，调用 supplier_search 工具一次，获取候选供应商列表，不要重复调用。
第二步，对搜索结果中的每个 supplierId，各调用一次 quote_request 获取报价（数量填写采购申请中的数量）。
第三步，汇总所有报价，以 JSON 数组格式返回，每条包含：supplierId（供应商ID）、supplierName（供应商名称）、unitPrice（单价）、leadTimeDays（交货期天数）、qualityScore（质量评分）、hasFrameContract（是否有框架协议）。', '请根据以下结构化需求，搜索合适的供应商并获取报价。', 'json', 120, 3, 'retry', '', 'warn', '', '2026-05-30 20:33:36.722914+08', '2026-06-01 09:17:11.569955+08', 'similarity', 5, 0.70, NULL, 'Agent', NULL, 10, NULL, 1536, NULL);
INSERT INTO public.ai_activity_config (id, process_id, version, activity_id, model_provider_id, model_name, description, temperature, max_tokens, system_prompt, user_message, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime, rag_search_strategy, rag_search_count, rag_similarity_threshold, rag_search_mode, service_type, rag_function, memory_turns, rag_embedding_model, rag_embedding_dimensions, rag_embedding_model_id) VALUES (21, 'Process_SmartProcurement_8529', '1', 'Activity_agent003', 3, 'qwen-plus', '合规审查智能体：检查适用法规认证要求，核查供应商黑名单状态', 0.30, 2048, '你是一位采购合规审查专家。请按以下步骤执行：第一步，调用 regulation_search 工具查询该品类适用的法规和认证要求；第二步，调用 blacklist_check 工具核查各供应商是否在制裁或内部黑名单中。最终以 JSON 格式返回合规报告：isCompliant（是否合规）、violations（违规项列表）、blacklistedSuppliers（黑名单供应商）、regulationSummary（法规摘要说明）。', '请对以下采购需求和候选供应商进行合规性审查。', 'json', 120, 3, 'retry', NULL, 'warn', NULL, '2026-05-30 12:33:36.722914+08', '2026-05-30 12:37:22.152657+08', 'similarity', 5, 0.70, 'hybrid', 'Agent', NULL, 10, NULL, 1536, NULL);
INSERT INTO public.ai_activity_config (id, process_id, version, activity_id, model_provider_id, model_name, description, temperature, max_tokens, system_prompt, user_message, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime, rag_search_strategy, rag_search_count, rag_similarity_threshold, rag_search_mode, service_type, rag_function, memory_turns, rag_embedding_model, rag_embedding_dimensions, rag_embedding_model_id) VALUES (22, 'Process_SmartProcurement_8529', '1', 'Activity_agent004', 3, 'qwen-plus', '价格评估智能体：计算各供应商全生命周期总成本（TCO），推荐最优采购方案', 0.30, 2048, '你是一位采购价格评估专家。请调用 tco_calculator 工具对各供应商进行总拥有成本分析（质量评分越高，TCO 折扣越大）。综合考量单价、交货期、质量评分和是否有框架协议，推荐最优供应商。以 JSON 格式返回推荐结果：recommendedSupplierId（推荐供应商ID）、recommendedSupplierName（供应商名称）、negotiatedUnitPrice（议价单价）、totalAmount（总金额）、rationale（推荐理由）、approvalRoute（审批路线）。', '请根据以下供应商报价和合规报告，评估并推荐最优采购方案。', 'json', 120, 3, 'retry', NULL, 'warn', NULL, '2026-05-30 12:33:36.722914+08', '2026-05-30 12:37:22.153202+08', 'similarity', 5, 0.70, 'hybrid', 'Agent', NULL, 10, NULL, 1536, NULL);
INSERT INTO public.ai_activity_config (id, process_id, version, activity_id, model_provider_id, model_name, description, temperature, max_tokens, system_prompt, user_message, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime, rag_search_strategy, rag_search_count, rag_similarity_threshold, rag_search_mode, service_type, rag_function, memory_turns, rag_embedding_model, rag_embedding_dimensions, rag_embedding_model_id) VALUES (23, 'Process_SmartProcurement_8529', '1', 'Activity_agent005', 3, 'qwen-plus', '下单执行智能体：在ERP系统中创建采购订单，返回订单编号和执行状态', 0.30, 2048, '你是一位ERP采购下单专员。请根据价格评估推荐结果，调用 create_purchase_order 工具在ERP系统中创建采购订单。确认订单创建成功后，返回订单详情，包括：PO编号、供应商名称、采购数量、单价、总金额和订单状态。如创建失败请说明原因。', '请根据以下采购推荐结果，在ERP系统中创建采购订单。', 'json', 120, 3, 'retry', NULL, 'warn', NULL, '2026-05-30 12:33:36.722914+08', '2026-05-30 12:37:22.15389+08', 'similarity', 5, 0.70, 'hybrid', 'Agent', NULL, 10, NULL, 1536, NULL);
INSERT INTO public.ai_activity_config (id, process_id, version, activity_id, model_provider_id, model_name, description, temperature, max_tokens, system_prompt, user_message, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime, rag_search_strategy, rag_search_count, rag_similarity_threshold, rag_search_mode, service_type, rag_function, memory_turns, rag_embedding_model, rag_embedding_dimensions, rag_embedding_model_id) VALUES (1, 'Process_ATGInquiry_2026', '1', 'Activity_agent001', 3, 'qwen-plus', 'LED照明需求解析智能体：从客户询价中提取产品规格、认证要求、应用场景等结构化信息', 0.20, 2048, '你是ATG Lighting的LED照明需求分析专家。请从客户询价描述中提取结构化信息。

执行步骤：
1. 调用 classify_product 工具识别产品类别（High Bay/Flood/Street/Panel/Canopy/Shoebox）
2. 调用 extract_specs 工具提取技术规格：功率(W)、色温(K)、防护等级(IP)、显色指数(CRI)、安装高度
3. 调用 check_certifications 工具确认认证要求（DLC/DLC Premium/Title 24/UL/ETL/CE）
4. 分析应用场景：仓库/停车场/道路/零售/工业/体育场馆

最终以JSON格式返回：productCategory、wattage、cct、ipRating、cri、height、certifications[]、application、quantity、targetPrice、deliveryDate、confidenceScore。

约束：不得推断未明确提及的信息；认证字段必须为客户明确要求或标准场景默认值。', '请分析以下客户询价，提取结构化的LED照明需求信息：{userInput}', 'json', 120, 3, 'retry', NULL, 'warn', NULL, '2026-06-02 09:27:35.756465+08', NULL, 'similarity', 5, 0.75, 'hybrid', 'Agent', NULL, 10, NULL, 1536, NULL);
INSERT INTO public.ai_activity_config (id, process_id, version, activity_id, model_provider_id, model_name, description, temperature, max_tokens, system_prompt, user_message, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime, rag_search_strategy, rag_search_count, rag_similarity_threshold, rag_search_mode, service_type, rag_function, memory_turns, rag_embedding_model, rag_embedding_dimensions, rag_embedding_model_id) VALUES (2, 'Process_ATGInquiry_2026', '1', 'Activity_agent002', 3, 'qwen-plus', 'ERP产品匹配智能体：根据需求规格在M3 ERP中匹配最合适的LED产品SKU并查询协议价格', 0.10, 2048, '你是ATG Lighting的ERP产品选型专家，负责将客户需求匹配到最合适的产品SKU。

执行步骤：
1. 调用 query_product_catalog 工具：输入{productCategory, wattage, ipRating, certifications}，返回候选SKU列表（最多5个）
2. 对每个候选SKU调用 query_erp_price(sku, quantity) 获取协议价格和阶梯折扣
3. 调用 check_stock(sku, warehouse) 确认库存可用量和预计补货日期
4. 调用 get_ies_data(sku) 获取光学参数验证是否满足照度要求
5. 综合评分选出最优3款产品（权重：规格匹配60%+价格优势25%+库存可用15%）

并行调用约束：步骤2、3、4可并行执行（Task.WhenAll）以加速响应。

输出JSON：matchedProducts[]（sku/productName/specification/unitPrice/availableStock/leadTime/matchScore）、recommendedSku（最推荐SKU）、matchReason。', '请根据以下需求分析结果，在ERP系统中匹配最合适的LED产品：{parsedRequirement}', 'json', 90, 2, 'retry', NULL, 'warn', NULL, '2026-06-02 09:27:36.02724+08', NULL, 'similarity', 5, 0.70, 'hybrid', 'Agent', NULL, 10, NULL, 1536, NULL);
INSERT INTO public.ai_activity_config (id, process_id, version, activity_id, model_provider_id, model_name, description, temperature, max_tokens, system_prompt, user_message, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime, rag_search_strategy, rag_search_count, rag_similarity_threshold, rag_search_mode, service_type, rag_function, memory_turns, rag_embedding_model, rag_embedding_dimensions, rag_embedding_model_id) VALUES (3, 'Process_ATGInquiry_2026', '1', 'Activity_agent003', 3, 'qwen-plus', 'AI报价生成智能体：生成专业LED照明报价单，含折扣策略、节能ROI计算和付款条款', 0.30, 3000, '你是ATG Lighting的资深报价专家，负责生成准确、专业且有竞争力的LED照明报价单。

执行步骤：
1. 调用 calculate_discount(quantity, basePrice) 计算数量折扣：
   - 1-49件：0%折扣（标准价）
   - 50-99件：3%折扣
   - 100-499件：5%折扣
   - 500+件：8%折扣
   - 老客户额外2%忠诚折扣
2. 调用 calculate_roi(oldWatts, newWatts, hoursPerDay, electricityRate, quantity) 计算节能收益：
   - 年节能度数 = (旧功率-新功率)/1000 × 运行小时 × 365 × 数量
   - 年节省金额 = 年节能度数 × 电费单价
   - 投资回收期(月) = 产品总投资 / (年节省金额/12)
   - CO₂减少量 = 年节能度数 × 0.42 kg/kWh
3. 调用 get_rebate_info(state, certifications) 查询DLC/State rebate补贴金额
4. 生成完整报价单结构

报价单要素：报价编号(QT-YYYYMMDD-XXX)、有效期30天、产品明细、折扣明细、节能ROI摘要、Rebate信息、付款条件(30%定金+70%出货前)、交期、质保条款(5年有限质保)。

约束：价格不得低于成本价的110%；报价必须包含DLC认证文件编号；关键业务字段(价格/SKU)幻觉率=0%。', '请根据以下ERP产品匹配结果和客户需求，生成专业的LED照明报价单：\n产品匹配：{productMatches}\n客户需求：{parsedRequirement}', 'json', 180, 3, 'retry', NULL, 'info', NULL, '2026-06-02 09:27:36.270015+08', NULL, 'similarity', 5, 0.70, 'hybrid', 'Agent', NULL, 10, NULL, 1536, NULL);
INSERT INTO public.ai_activity_config (id, process_id, version, activity_id, model_provider_id, model_name, description, temperature, max_tokens, system_prompt, user_message, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime, rag_search_strategy, rag_search_count, rag_similarity_threshold, rag_search_mode, service_type, rag_function, memory_turns, rag_embedding_model, rag_embedding_dimensions, rag_embedding_model_id) VALUES (24, 'Process_ATGInquiry_2026', '1', 'Activity_agent004', 3, 'qwen-plus', 'PDF交付智能体：生成专业报价PDF发送客户，写入CRM创建跟进任务', 0.20, 1500, '你是ATG Lighting的客户交付专员。步骤（1和2并行执行）：
1. generate_pdf(quoteData,"atg_professional") → 含Logo/DLC标识/节能ROI图表的专业PDF
2. compose_email() → 英文邮件，主题"Your Quotation {quoteNo} from ATG Lighting"
3. send_email(toEmail, emailContent, pdfAttachment) → 发送
4. create_crm_lead() → CRM录入，标签：已发报价/LED-{category}，设3天跟进提醒
5. notify_sales() → 通知销售
约束：审批后30分钟内发出；CRM记录必须完整。', '为审批通过的报价单生成PDF：报价单={approvedQuote}，客户邮箱={customerEmail}', 'json', 120, 3, 'retry', NULL, 'info', NULL, '2026-06-02 09:28:43.625247+08', NULL, 'similarity', 5, 0.70, 'hybrid', 'Agent', NULL, 5, NULL, 1536, NULL);
INSERT INTO public.ai_activity_config (id, process_id, version, activity_id, model_provider_id, model_name, description, temperature, max_tokens, system_prompt, user_message, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime, rag_search_strategy, rag_search_count, rag_similarity_threshold, rag_search_mode, service_type, rag_function, memory_turns, rag_embedding_model, rag_embedding_dimensions, rag_embedding_model_id) VALUES (25, 'Process_ATGInquiry_2026', '1', 'Activity_agent005', 3, 'qwen-plus', '报价重生成智能体：根据销售驳回原因调整策略，重新生成优化报价', 0.40, 2048, '你是ATG Lighting报价优化专家。根据驳回原因执行对应策略：
- 价格偏高 → apply_special_discount(competitive,最高再降5%)
- 产品不匹配 → 扩大搜索±20%功率范围重新query_product_catalog
- 认证缺失 → check_pending_certifications查在途认证
- 交期太长 → query_stock_all_warehouses查全仓库
- 条款不合理 → get_payment_terms_options获取备选付款方案
调整后备注注明改动。最多2次驳回，每次须有实质性改变，不低于最低利润线。', '驳回原因：{rejectReason}；原报价：{previousQuote}；请优化并重新生成报价。', 'json', 150, 2, 'retry', NULL, 'warn', NULL, '2026-06-02 09:28:43.625247+08', NULL, 'similarity', 5, 0.70, 'hybrid', 'Agent', NULL, 10, NULL, 1536, NULL);


--
-- Data for Name: ai_agent; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.ai_agent (id, agent_name, agent_identifier, description, category, version, icon, is_active, status, model_provider, model_name, model_service, model_version, temperature, max_tokens, api_endpoint, api_key, system_prompt, few_shot_prompt, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime) VALUES (1, '客户服务助手', NULL, '为客户提供24/7在线支持，解答常见问题，处理简单请求。', NULL, 'v2.1', 'fas fa-robot', 1, 'running', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '2023-08-10 00:00:00+08', '2025-09-06 21:43:14.141+08');
INSERT INTO public.ai_agent (id, agent_name, agent_identifier, description, category, version, icon, is_active, status, model_provider, model_name, model_service, model_version, temperature, max_tokens, api_endpoint, api_key, system_prompt, few_shot_prompt, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime) VALUES (2, '数据分析助手', NULL, '自动分析业务数据，生成可视化报表，提供业务洞察。', NULL, 'v1.5', 'fas fa-chart-bar', 1, 'running', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '2023-07-22 00:00:00+08', '2025-09-06 21:43:14.141+08');
INSERT INTO public.ai_agent (id, agent_name, agent_identifier, description, category, version, icon, is_active, status, model_provider, model_name, model_service, model_version, temperature, max_tokens, api_endpoint, api_key, system_prompt, few_shot_prompt, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime) VALUES (3, '销售推荐引擎', NULL, '基于用户行为和偏好提供个性化产品推荐。', NULL, 'v3.0', 'fas fa-shopping-cart', 1, 'tesing', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '2023-09-01 00:00:00+08', '2025-09-06 21:43:14.141+08');
INSERT INTO public.ai_agent (id, agent_name, agent_identifier, description, category, version, icon, is_active, status, model_provider, model_name, model_service, model_version, temperature, max_tokens, api_endpoint, api_key, system_prompt, few_shot_prompt, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime) VALUES (4, '多语言翻译', NULL, '支持50+语言的实时翻译服务，保持上下文一致性。', NULL, 'v2.3', 'fas fa-language', 1, 'running', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '2023-08-15 00:00:00+08', '2025-09-06 21:43:14.141+08');
INSERT INTO public.ai_agent (id, agent_name, agent_identifier, description, category, version, icon, is_active, status, model_provider, model_name, model_service, model_version, temperature, max_tokens, api_endpoint, api_key, system_prompt, few_shot_prompt, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime) VALUES (5, '安全监控助手', NULL, '实时监控系统安全，检测异常行为并发出警报。', NULL, 'v1.2', 'fas fa-shield-alt', 1, 'running', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '2023-08-25 00:00:00+08', '2025-09-06 21:43:14.141+08');
INSERT INTO public.ai_agent (id, agent_name, agent_identifier, description, category, version, icon, is_active, status, model_provider, model_name, model_service, model_version, temperature, max_tokens, api_endpoint, api_key, system_prompt, few_shot_prompt, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime) VALUES (6, '文档生成器', NULL, '根据数据自动生成专业文档和报告。', NULL, 'v2.0', 'fas fa-file-alt', 1, 'planning', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '2023-09-05 00:00:00+08', '2025-09-06 21:43:14.141+08');
INSERT INTO public.ai_agent (id, agent_name, agent_identifier, description, category, version, icon, is_active, status, model_provider, model_name, model_service, model_version, temperature, max_tokens, api_endpoint, api_key, system_prompt, few_shot_prompt, response_format, time_out, max_retries, error_handling, fallback_agent, log_level, custom_instructions, created_datetime, updated_datetime) VALUES (17, '需求分析智能体', 'requirement_analyzer', '分析用户需求并生成清晰、结构化的需求规格文档。能够理解自然语言描述的业务需求。', '需求分析', '1.2.0', NULL, 0, NULL, NULL, NULL, 'Azure OpenAI', 'gpt-4-turbo', 0.30, 4096, 'https://api.example.com/v1', 'sk-****************', '你是一个资深产品经理，负责将模糊的用户需求转化为清晰的需求规格文档。你需要：

1. 理解用户描述的业务需求
2. 识别关键业务流程和功能点
3. 将需求分解为具体的用户故事和功能点
4. 输出结构化的需求规格文档（JSON格式）', '用户输入：我需要一个电商网站，用户可以浏览商品、加入购物车和下单。

输出示例：
{
  "功能列表": ["商品浏览", "购物车管理", "下单流程"],
  "用户故事": [
    {"角色": "访客", "操作": "浏览商品列表"},
    {"角色": "用户", "操作": "将商品加入购物车"}
  ]
}', 'json', 30, 9, 'retry', 'backup_analyzer', 'warn', '在处理金融领域需求时，特别关注合规性和安全性要求。', '2001-01-01 00:00:00+08', '2001-01-01 00:00:00+08');


--
-- Data for Name: ai_agent_parameter; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.ai_agent_parameter (id, agent_id, parameter_name, direction, data_type, is_required, description, default_value, order_index) VALUES (22, 17, 'user_query', 'input', 'object', 0, NULL, NULL, 0);
INSERT INTO public.ai_agent_parameter (id, agent_id, parameter_name, direction, data_type, is_required, description, default_value, order_index) VALUES (23, 17, 'specification', 'output', 'object', 0, NULL, NULL, 0);


--
-- Data for Name: ai_model_provider; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.ai_model_provider (id, model_provider, base_url, api_uuid, api_key, created_datetime, updated_datetime, is_active, description, model_type, model_name) VALUES (1, 'OpenAI', 'https://api.openai.com/v1', '882187bb-1eec-4425-971e-ba278de938ce', 'WIcR+0tkz81BaTwWv3Hy4cC4jc5jzX3JMFWMWG6OQsSrveEJbwDELbP05sQFiTjgIxrWe4qKQB79Dwctz9GuzJ+06OVSUnmR8nCVi4NYev9/0hkq9W2MbJqrnL0WfqDE4ZfjoC4NmT4IjKVitIA67W4uGZtpLlStpe9fq9w7tKnyvJ6TA+VUL1To5S/LZPdyE8c1xAhC/z1tI4uCD3jMD+MO7iKU7ppM54WhcGbpoBhUBC78iiopyHlK3iUGfL5t', '2025-12-12 02:37:04.791825+08', '2026-02-26 16:23:30.666188+08', true, '', 'text_generation', 'gpt-4o');
INSERT INTO public.ai_model_provider (id, model_provider, base_url, api_uuid, api_key, created_datetime, updated_datetime, is_active, description, model_type, model_name) VALUES (2, 'QWen', 'https://dashscope.aliyuncs.com/compatible-mode/v1/embeddings', 'b89177b7-42fe-45f8-8b4a-646936466c4d', 'wOzAvxitIw0Zmmz4Ve4FAxGDrjWdNLOYIWhxdIfcjcjjlkIXd464LTswKQRQKpbQu9uh9gVbCZvD5y4ixbCY', '2026-02-27 15:51:38.464511+08', '2026-02-26 16:27:25.731264+08', true, 'vector model', 'vector_model', 'text-embedding-v4');
INSERT INTO public.ai_model_provider (id, model_provider, base_url, api_uuid, api_key, created_datetime, updated_datetime, is_active, description, model_type, model_name) VALUES (3, 'QWen', 'https://dashscope.aliyuncs.com/compatible-mode/v1/chat/completions', '1dc19d29-56dc-45f5-ad26-3ed843b6100f', 'zUFegah6aukxeTYuAZ2bZai1dtCjYfQl1b0GLMrHZ4fgj2MA+A3RvsCxgeCU2hWuC0UfpqHdCUOwv6htvaxE', '2026-02-27 00:28:29.201803+08', '2026-06-20 20:09:55.731825+08', true, 'qwen text generation', 'text_generation', 'qwen3.7-plus');


--
-- Data for Name: biz_app_flow; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (113, '流程发起', '3', NULL, NULL, '流程发起', 'mssqlserver申请人:6-普通员工-小明', '2015-08-15 13:21:29.41+08', '6', '普通员工-小明');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (114, '生产订单', '624', 'TB300427', NULL, '派单', '完成派单', '2015-08-15 16:17:19.127+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (115, '生产订单', '625', 'TB906432', NULL, '派单', '完成派单', '2015-08-15 16:17:50.613+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (116, '生产订单', '626', 'TB338322', NULL, '派单', '完成派单', '2015-08-15 16:18:04.003+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (117, '生产订单', '627', 'TB612344', NULL, '派单', '完成派单', '2015-08-15 20:14:43.38+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (118, '生产订单', '628', 'TB683061', NULL, '派单', '完成派单', '2015-08-15 20:14:51.38+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (119, '生产订单', '628', 'TB683061', NULL, '打样', '完成打样', '2015-08-15 20:15:14.05+08', '11', '打样员-飞雨');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (120, '生产订单', '627', 'TB612344', NULL, '打样', '完成打样', '2015-08-15 20:15:22.26+08', '11', '打样员-飞雨');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (121, '生产订单', '627', 'TB612344', NULL, '生产', '完成生产', '2015-08-17 12:57:00.483+08', '9', '跟单员-张明');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (122, '生产订单', '631', 'TB490683', NULL, '派单', '完成派单', '2015-08-19 15:17:06.52+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (123, '生产订单', '630', 'TB351094', NULL, '派单', '完成派单', '2015-08-22 22:01:51.677+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (124, '生产订单', '632', 'TB366615', NULL, '派单', '完成派单', '2015-08-25 14:58:21.407+08', '8', '业务员-小宋');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (125, '生产订单', '634', 'TB969829', NULL, '派单', '完成派单', '2015-08-25 14:58:29.163+08', '8', '业务员-小宋');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (126, '生产订单', '633', 'TB751853', NULL, '派单', '完成派单', '2015-08-25 23:24:35.317+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (127, '生产订单', '639', 'TB792242', NULL, '派单', '完成派单', '2015-08-26 16:58:09.947+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (128, '生产订单', '639', 'TB792242', NULL, '打样', '完成打样', '2015-08-27 08:29:27.367+08', '11', '打样员-飞雨');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (129, '生产订单', '640', 'TB429545', NULL, '派单', '完成派单', '2015-09-05 16:21:22.963+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (130, '生产订单', '641', 'TB817384', NULL, '派单', '完成派单', '2015-09-06 10:53:34.06+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (131, '生产订单', '644', 'TB348804', NULL, '派单', '完成派单', '2015-09-06 13:23:53.743+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (132, '生产订单', '643', 'TB351670', NULL, '派单', '完成派单', '2015-09-06 13:23:58.763+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (133, '生产订单', '646', 'TB992099', NULL, '派单', '完成派单', '2015-09-06 13:51:14.847+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (134, '生产订单', '648', 'TB588606', NULL, '派单', '完成派单', '2015-09-06 14:15:29.943+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (135, '生产订单', '642', 'TB434232', NULL, '派单', '完成派单', '2015-09-07 17:31:09.793+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (136, '生产订单', '647', 'TB285386', NULL, '派单', '完成派单', '2015-09-10 09:52:59.46+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (137, '生产订单', '652', 'TB991726', NULL, '派单', '完成派单', '2015-09-11 21:30:45.453+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (138, '生产订单', '652', 'TB991726', NULL, '打样', '完成打样', '2015-09-11 21:31:51.35+08', '11', '打样员-飞雨');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (139, '生产订单', '652', 'TB991726', NULL, '生产', '完成生产', '2015-09-11 21:32:53.023+08', '10', '跟单员-李杰');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (140, '生产订单', '651', 'TB728743', NULL, '派单', '完成派单', '2015-09-14 16:11:58.53+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (141, '生产订单', '650', 'TB328175', NULL, '派单', '完成派单', '2015-09-14 16:12:02.31+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (142, '流程发起', '4', NULL, NULL, '流程发起', '申请人:6-普通员工-小明', '2015-10-08 18:12:42.08+08', '6', '普通员工-小明');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (143, '流程发起', '5', NULL, NULL, '流程发起', '申请人:6-普通员工-小明', '2015-10-09 08:51:33.66+08', '6', '普通员工-小明');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (144, '流程发起', '6', NULL, NULL, '流程发起', '申请人:6-普通员工-小明', '2015-10-09 16:08:56.34+08', '6', '普通员工-小明');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (145, '请假流程', '6', NULL, NULL, '部门经理审批', '部门经理-张(ID:5) 同意', '2015-10-09 16:49:14.623+08', '5', '部门经理-张');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (146, '生产订单', '659', 'TB710707', NULL, '派单', '完成派单', '2015-12-24 19:16:36.857+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (147, '生产订单', '658', 'TB575859', NULL, '派单', '完成派单', '2015-12-24 20:23:41.287+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (148, '生产订单', '659', 'TB710707', NULL, '打样', '完成打样', '2015-12-24 20:24:01.77+08', '11', '打样员-飞雨');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (149, '生产订单', '657', 'TB358232', NULL, '派单', '完成派单', '2015-12-24 21:49:22.71+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (150, '生产订单', '656', 'TB779780', NULL, '派单', '完成派单', '2015-12-26 17:32:34.37+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (151, '生产订单', '655', 'TB322602', NULL, '派单', '完成派单', '2015-12-28 20:08:35.1+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (152, '生产订单', '654', 'TB271916', NULL, '派单', '完成派单', '2015-12-28 20:12:58.433+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (153, '生产订单', '654', 'TB271916', NULL, '打样', '完成打样', '2015-12-28 20:14:23.047+08', '11', '打样员-飞雨');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (154, '生产订单', '653', 'TB559248', NULL, '派单', '完成派单', '2015-12-29 18:11:30.313+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (155, '生产订单', '649', 'TB771229', NULL, '派单', '完成派单', '2015-12-29 20:12:36.253+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (158, '生产订单', '645', 'TB642095', NULL, '派单', '完成派单', '2015-12-29 21:29:36.663+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (159, '生产订单', '660', 'TB967961', NULL, '派单', '完成派单', '2015-12-29 21:32:14.467+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (160, '生产订单', '661', 'TB751700', NULL, '派单', '完成派单', '2015-12-29 21:38:00.827+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (161, '生产订单', '661', 'TB751700', NULL, '打样', '完成打样', '2015-12-29 21:38:21.593+08', '11', '打样员-飞雨');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (162, '生产订单', '661', 'TB751700', NULL, '生产', '完成生产', '2015-12-29 21:38:42.03+08', '9', '跟单员-张明');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (163, '生产订单', '661', 'TB751700', NULL, '质检', '完成质检', '2015-12-29 21:39:00+08', '13', '质检员-杰米');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (164, '生产订单', '661', 'TB751700', NULL, '称重', '完成称重', '2015-12-29 21:41:35.763+08', '15', '包装员-大汉');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (165, '生产订单', '661', 'TB751700', NULL, '发货', '完成发货', '2015-12-29 21:41:54.12+08', '15', '包装员-大汉');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (166, '生产订单', '652', 'TB991726', NULL, '派单', '完成派单', '2015-12-30 20:02:36.133+08', '8', '业务员-小宋');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (167, '生产订单', '662', 'TB647767', NULL, '派单', '完成派单', '2015-12-30 21:56:46.383+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (168, '生产订单', '638', 'TB561443', NULL, '派单', '完成派单', '2015-12-31 19:10:06.787+08', '8', '业务员-小宋');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (169, '生产订单', '663', 'TB809544', NULL, '派单', '完成派单', '2015-12-31 19:12:09.783+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (170, '生产订单', '664', 'TB914891', NULL, '派单', '完成派单', '2015-12-31 19:13:46.283+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (171, '生产订单', '665', 'TB929075', NULL, '派单', '完成派单', '2015-12-31 19:43:35.82+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (172, '生产订单', '666', 'TB225725', NULL, '派单', '完成派单', '2015-12-31 19:50:25.05+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (173, '生产订单', '667', 'TB164370', NULL, '派单', '完成派单', '2015-12-31 19:52:18.3+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (174, '生产订单', '667', 'TB164370', NULL, '打样', '完成打样', '2015-12-31 19:53:38.493+08', '11', '打样员-飞雨');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (175, '生产订单', '667', 'TB164370', NULL, '生产', '完成生产', '2015-12-31 19:53:58.467+08', '9', '跟单员-张明');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (176, '生产订单', '667', 'TB164370', NULL, '质检', '完成质检', '2015-12-31 19:54:13.17+08', '13', '质检员-杰米');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (177, '生产订单', '667', 'TB164370', NULL, '称重', '完成称重', '2015-12-31 19:54:30.21+08', '15', '包装员-大汉');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (178, '生产订单', '667', 'TB164370', NULL, '发货', '完成发货', '2015-12-31 19:55:04.907+08', '16', '包装员-小威');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (179, '流程发起', '7', NULL, NULL, '流程发起', '申请人:6-普通员工-小明', '2016-02-25 10:48:27.977+08', '6', '普通员工-小明');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (180, '请假流程', '7', NULL, NULL, '部门经理审批', '部门经理-张(ID:5) 同意', '2016-02-25 10:49:15.247+08', '5', '部门经理-张');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (181, '请假流程', '7', NULL, NULL, '总经理审批', '总经理-陈(ID:1) 同意', '2016-02-25 10:49:42.1+08', '1', '总经理-陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (182, '请假流程', '7', NULL, NULL, '人事经理审批', '人事经理-李小姐(ID:4) ', '2016-02-25 10:50:02.52+08', '4', '人事经理-李小姐');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (183, '流程发起', '8', NULL, NULL, '流程发起', '申请人:6-普通员工-小明', '2016-02-25 10:53:40.977+08', '6', '普通员工-小明');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (184, '请假流程', '8', NULL, NULL, '部门经理审批', '部门经理-张(ID:5) 同意', '2016-02-25 10:54:09.017+08', '5', '部门经理-张');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (185, '生产订单', '669', 'TB747473', NULL, '派单', '完成派单', '2016-02-25 10:55:01.283+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (186, '生产订单', '669', 'TB747473', NULL, '打样', '完成打样', '2016-02-25 10:55:18.963+08', '11', '打样员-飞雨');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (187, '生产订单', '670', 'TB630627', NULL, '派单', '完成派单', '2016-02-25 10:56:28.487+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (188, '生产订单', '670', 'TB630627', NULL, '打样', '完成打样', '2016-02-25 10:56:49.137+08', '11', '打样员-飞雨');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (189, '生产订单', '670', 'TB630627', NULL, '生产', '完成生产', '2016-02-25 10:57:09.807+08', '9', '跟单员-张明');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (190, '生产订单', '670', 'TB630627', NULL, '质检', '完成质检', '2016-02-25 10:57:27.59+08', '13', '质检员-杰米');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (191, '生产订单', '670', 'TB630627', NULL, '称重', '完成称重', '2016-02-25 10:57:44.987+08', '15', '包装员-大汉');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (192, '生产订单', '670', 'TB630627', NULL, '发货', '完成发货', '2016-02-25 10:58:09.573+08', '15', '包装员-大汉');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (193, '生产订单', '671', 'TB165916', NULL, '派单', '完成派单', '2016-03-10 09:28:10.767+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (194, '流程发起', '9', NULL, NULL, '流程发起', '申请人:6-普通员工-小明', '2016-03-10 09:45:36.157+08', '6', '普通员工-小明');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (195, '流程发起', '10', NULL, NULL, '流程发起', '申请人:6-普通员工-小明', '2016-03-10 10:56:13.423+08', '6', '普通员工-小明');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (196, '流程发起', '11', NULL, NULL, '流程发起', '申请人:6-普通员工-小明', '2016-03-10 15:26:53.043+08', '6', '普通员工-小明');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (197, '生产订单', '673', 'TB508950', NULL, '派单', '完成派单', '2016-05-27 14:28:45.57+08', '7', ' 业务员-小陈');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (198, '生产订单', '673', 'TB508950', NULL, '打样', '完成打样', '2016-05-27 14:29:10.153+08', '11', '打样员-飞雨');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (199, '生产订单', '673', 'TB508950', NULL, '生产', '完成生产', '2016-05-27 14:29:35.4+08', '9', '跟单员-张明');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (200, '生产订单', '674', 'TB760538', NULL, '派单', '完成派单', '2016-06-27 15:35:25.303+08', '7', '陈盖茨');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (201, '生产订单', '674', 'TB760538', NULL, '生产', '完成生产', '2016-06-27 16:39:25.747+08', '11', '飞羽');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (202, '生产订单', '672', 'TB247595', NULL, '派单', '完成派单', '2016-09-10 21:05:21.19+08', '7', '陈盖茨');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (203, '生产订单', '668', 'TB885696', NULL, '派单', '完成派单', '2017-03-01 15:02:30.12+08', '7', '陈盖茨');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (204, '生产订单', '675', 'TB324384', NULL, '派单', '完成派单', '2017-03-01 15:04:08.2+08', '7', '陈盖茨');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (205, '生产订单', '675', 'TB324384', NULL, '打样', '完成打样', '2017-03-01 15:27:10.497+08', '11', '飞羽');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (206, '生产订单', '675', 'TB324384', NULL, '生产', '完成生产', '2017-03-01 15:33:14.603+08', '9', '张明');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (207, '生产订单', '675', 'TB324384', NULL, '质检', '完成质检', '2017-03-01 15:33:36.23+08', '13', '杰米');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (208, '生产订单', '675', 'TB324384', NULL, '称重', '完成称重', '2017-03-01 15:33:52.337+08', '15', '大汉');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (209, '生产订单', '675', 'TB324384', NULL, '发货', '完成发货', '2017-03-01 15:34:05.057+08', '15', '大汉');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (210, '流程发起', '12', NULL, NULL, '流程发起', '申请人:6-路天明', '2017-03-01 15:46:21.197+08', '6', '路天明');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (211, '请假流程', '12', NULL, NULL, '部门经理审批', '张恒丰(ID:5) 同意', '2017-03-01 15:46:48.447+08', '5', '张恒丰');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (212, '请假流程', '12', NULL, NULL, '人事经理审批', '李颖(ID:4) ', '2017-03-01 15:47:26.623+08', '4', '李颖');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (213, '流程发起', '13', NULL, NULL, '流程发起', '申请人:6-路天明', '2017-03-14 13:47:33.603+08', '6', '路天明');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (214, '请假流程', '13', NULL, NULL, '部门经理审批', '张恒丰(ID:5) AGREE', '2017-03-14 13:47:56.203+08', '5', '张恒丰');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (215, '请假流程', '13', NULL, NULL, '人事经理审批', '李颖(ID:4) ', '2017-03-14 13:48:11.873+08', '4', '李颖');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (216, '生产订单', '676', 'TB377329', NULL, '派单', '完成派单', '2017-06-12 12:26:45.67+08', '7', '陈盖茨');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (217, '流程发起', '32', NULL, NULL, '流程发起', '申请人:6-路天明', '2017-07-22 09:09:49.76+08', '6', '路天明');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (218, '请假流程', '32', NULL, NULL, '部门经理审批', '张恒丰(ID:5) 同意', '2017-07-22 09:12:20.057+08', '5', '张恒丰');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (219, '流程发起', '33', NULL, NULL, '流程发起', '申请人:6-路天明', '2017-07-22 09:27:49.403+08', '6', '路天明');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (220, '生产订单', '678', 'TB574787', NULL, '派单', '完成派单', '2017-07-22 09:36:25.903+08', '7', '陈盖茨');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (221, '生产订单', '679', 'TB100834', NULL, '派单', '完成派单', '2017-08-23 19:07:04.19+08', '7', '陈盖茨');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (222, '生产订单', '679', 'TB100834', NULL, '打样', '完成打样', '2017-08-23 19:07:21.627+08', '11', '飞羽');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (223, '生产订单', '680', 'TB752624', NULL, '派单', '完成派单', '2017-12-04 11:06:04.08+08', '7', '陈盖茨');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (224, '生产订单', '680', 'TB752624', NULL, '打样', '完成打样', '2017-12-04 11:06:22.89+08', '11', '飞羽');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (225, '生产订单', '680', 'TB752624', NULL, '生产', '完成生产', '2017-12-04 11:06:35.53+08', '9', '张明');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (226, '生产订单', '680', 'TB752624', NULL, '质检', '完成质检', '2017-12-04 11:06:48.64+08', '13', '杰米');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (227, '生产订单', '680', 'TB752624', NULL, '发货', '完成发货', '2017-12-04 11:07:04.937+08', '15', '大汉');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (228, '生产订单', '680', 'TB752624', NULL, '发货', '完成发货', '2017-12-04 11:07:26.283+08', '16', '小威');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (229, '生产订单', '681', 'TB517477', NULL, '派单', '完成派单', '2017-12-04 13:56:47.49+08', '7', '陈盖茨');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (230, '生产订单', '681', 'TB265497', NULL, '派单', '完成派单', '2017-12-07 16:13:31.423+08', '7', '陈盖茨');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (231, '生产订单', '682', 'TB601588', NULL, '派单', '完成派单', '2017-12-07 16:14:12.397+08', '7', '陈盖茨');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (232, '生产订单', '682', 'TB601588', NULL, '打样', '完成打样', '2017-12-07 16:14:26.523+08', '11', '飞羽');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (233, '生产订单', '677', 'TB730548', NULL, '派单', '完成派单', '2018-06-17 16:56:33.217+08', '7', '陈盖茨');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (234, '生产订单', '684', 'TB937073', NULL, '派单', '完成派单', '2019-03-06 21:22:34.097+08', '7', '陈盖茨');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (235, '生产订单', '685', 'TB359987', NULL, '派单', '完成派单', '2019-10-30 14:31:17.373+08', '11', '飞羽');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (236, '生产订单', '686', 'TB588656', NULL, '派单', '完成派单', '2020-05-15 10:52:38.177+08', '7', 'Gates');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (237, '生产订单', '686', 'TB588656', NULL, '打样', '完成打样', '2020-05-15 10:53:01.473+08', '11', 'FeiYu');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (238, '生产订单', '686', 'TB588656', NULL, '生产', '完成生产', '2020-05-15 10:53:21.953+08', '9', 'ZhangMing');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (239, '生产订单', '686', 'TB588656', NULL, '质检', '完成质检', '2020-05-15 10:53:40.107+08', '13', 'Jimi');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (240, '生产订单', '686', 'TB588656', NULL, '称重', '完成称重', '2020-05-15 10:53:57.853+08', '15', 'DaHan');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (241, '生产订单', '686', 'TB588656', NULL, '发货', '完成发货', '2020-05-15 10:54:05.27+08', '15', 'DaHan');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (242, '生产订单', '687', 'TB720748', NULL, '派单', '完成派单', '2020-08-26 15:11:01.707+08', '7', 'Gates');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (243, '生产订单', '687', 'TB720748', NULL, '打样', '完成打样', '2020-08-26 15:16:19.77+08', '11', 'FeiYu');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (244, '生产订单', '687', 'TB720748', NULL, '生产', '完成生产', '2020-08-26 15:16:40.18+08', '9', 'ZhangMing');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (245, '生产订单', '687', 'TB720748', NULL, '质检', '完成质检', '2020-08-26 15:17:02.983+08', '13', 'Jimi');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (246, '生产订单', '687', 'TB720748', NULL, '发货', '完成发货', '2020-08-26 15:17:21.49+08', '15', 'DaHan');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (247, '生产订单', '687', 'TB720748', NULL, '发货', '完成发货', '2020-08-26 15:17:36.503+08', '15', 'DaHan');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (248, '生产订单', '688', 'TB332639', NULL, '派单', '完成派单', '2021-01-14 09:43:25.57+08', '7', 'Gates');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (249, '生产订单', '689', 'TB741954', NULL, '派单', '完成派单', '2021-01-14 09:49:32.437+08', '7', 'Gates');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (250, '生产订单', '689', 'TB741954', NULL, '打样', '完成打样', '2021-01-14 09:49:51.35+08', '11', 'FeiYu');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (251, '生产订单', '689', 'TB741954', NULL, '生产', '完成生产', '2021-01-14 09:50:06.79+08', '9', 'ZhangMing');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (252, '生产订单', '689', 'TB741954', NULL, '质检', '完成质检', '2021-01-14 09:50:20.98+08', '13', 'Jimi');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (253, '生产订单', '689', 'TB741954', NULL, '称重', '完成称重', '2021-01-14 09:50:36.133+08', '15', 'DaHan');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (254, '生产订单', '689', 'TB741954', NULL, '发货', '完成发货', '2021-01-14 09:50:48.747+08', '15', 'DaHan');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (255, '生产订单', '690', 'TB332806', NULL, '派单', '完成派单', '2021-08-25 15:33:20.34+08', '7', 'Peter');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (256, '生产订单', '683', 'TB393078', NULL, '派单', '完成派单', '2022-03-22 10:27:34.88+08', '7', 'Peter');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (257, '生产订单', '691', 'TB452818', NULL, '派单', '完成派单', '2022-03-22 10:28:11.997+08', '8', 'Bill');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (258, '生产订单', '693', 'TB307954', NULL, '派单', '完成派单', '2022-04-25 14:36:49.48+08', '7', 'Peter');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (259, '生产订单', '693', 'TB307954', NULL, '打样', '完成打样', '2022-04-25 14:37:14.493+08', '11', 'Fisher');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (260, '生产订单', '693', 'TB307954', NULL, '生产', '完成生产', '2022-04-25 14:37:30.403+08', '9', 'Tuda');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (261, '生产订单', '693', 'TB307954', NULL, '质检', '完成质检', '2022-04-25 14:38:03.153+08', '13', 'Jimi');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (262, '生产订单', '693', 'TB307954', NULL, '称重', '完成称重', '2022-04-25 14:38:18.393+08', '16', 'Smith');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (263, '生产订单', '693', 'TB307954', NULL, '发货', '完成发货', '2022-04-25 14:38:34.857+08', '16', 'Smith');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (268, '生产订单', '692', 'TB263682', NULL, '派单', '完成派单', '2022-07-09 08:35:56.193+08', '7', 'Peter');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (269, '生产订单', '692', 'TB263682', NULL, '打样', '完成打样', '2022-07-09 08:36:18.337+08', '11', 'Fisher');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (270, '生产订单', '692', 'TB263682', NULL, '生产', '完成生产', '2022-07-09 08:36:34.687+08', '9', 'Tuda');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (271, '生产订单', '692', 'TB263682', NULL, '质检', '完成质检', '2022-07-09 08:36:52.983+08', '13', 'Jimi');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (272, '生产订单', '692', 'TB263682', NULL, '称重', '完成称重', '2022-07-09 08:41:44.517+08', '15', 'Damark');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (273, '生产订单', '692', 'TB263682', NULL, '发货', '完成发货', '2022-07-09 08:42:04.943+08', '15', 'Damark');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (274, '生产订单', '694', 'TB293064', NULL, '派单', '完成派单', '2022-07-10 20:01:39.56+08', '7', 'Peter');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (275, '生产订单', '695', 'TB226532', NULL, '派单', '完成派单', '2024-02-04 16:02:52.85+08', '7', 'Peter');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (276, '生产订单', '696', 'TB207310', NULL, '派单', '完成派单', '2024-06-21 09:16:23.09+08', '7', 'Peter');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (277, '生产订单', '696', 'TB207310', NULL, '打样', '完成打样', '2024-06-21 10:22:51.917+08', '11', 'Fisher');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (278, '生产订单', '696', 'TB207310', NULL, '生产', '完成生产', '2024-06-21 10:23:41.27+08', '9', 'Tuda');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (279, '生产订单', '696', 'TB207310', NULL, '质检', '完成质检', '2024-06-21 10:24:12.697+08', '13', 'Jimi');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (280, '生产订单', '696', 'TB207310', NULL, '发货', '完成发货', '2024-06-21 10:24:29.977+08', '15', 'Damark');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (281, '生产订单', '696', 'TB207310', NULL, '发货', '完成发货', '2024-06-21 10:24:42.91+08', '15', 'Damark');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (282, '生产订单', '697', 'TB257249', NULL, '派单', '完成派单', '2024-06-21 11:00:38.927+08', '7', 'Peter');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (283, '生产订单', '697', 'TB257249', NULL, '打样', '完成打样', '2024-06-21 11:01:09.443+08', '11', 'Fisher');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (284, '生产订单', '698', 'TB409086', NULL, '派单', '完成派单', '2024-06-21 13:46:15.3+08', '7', 'Peter');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (285, '生产订单', '698', 'TB409086', NULL, '打样', '完成打样', '2024-06-21 13:46:53.493+08', '11', 'Fisher');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (292, 'ProductOrder', '699', 'TB849499', NULL, 'Disptach', 'Dispatch Completed', '2025-02-22 20:53:02.967+08', '7', 'Peter');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (293, 'ProductOrder', '699', 'TB849499', NULL, 'Sample', 'Sample Completed', '2025-02-22 20:53:25.887+08', '11', 'Fisher');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (294, 'ProductOrder', '699', 'TB849499', NULL, 'Manufacture', 'Manufacture Completed', '2025-02-22 20:53:41.573+08', '9', 'Tuda');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (295, 'ProductOrder', '699', 'TB849499', NULL, 'QC check', 'QC check complted', '2025-02-22 20:53:57.717+08', '13', 'Jimi');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (296, 'ProductOrder', '699', 'TB849499', NULL, 'Weight', 'Weight Completed', '2025-02-22 20:54:12.56+08', '15', 'Damark');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (298, 'ProductOrder', '700', 'TB751527', NULL, 'Disptach', 'Dispatch Completed', '2025-09-03 08:38:34.625+08', '7', 'Peter');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (299, 'ProductOrder', '700', 'TB751527', NULL, 'Sample', 'Sample Completed', '2025-09-03 08:38:52.2+08', '11', 'Fisher');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (300, 'ProductOrder', '700', 'TB751527', NULL, 'Manufacture', 'Manufacture Completed', '2025-09-03 08:39:08.508+08', '9', 'Tuda');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (301, 'ProductOrder', '700', 'TB751527', NULL, 'QC check', 'QC check complted', '2025-09-03 08:39:30.771+08', '13', 'Jimi');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (302, 'ProductOrder', '700', 'TB751527', NULL, 'Weight', 'Weight Completed', '2025-09-03 08:39:49.047+08', '15', 'Damark');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (304, 'ProductOrder', '704', 'TB649431', NULL, 'Disptach', 'Dispatch Completed', '2025-12-21 11:16:08.018595+08', '7', 'Peter');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (305, 'ProductOrder', '704', 'TB649431', NULL, 'Sample', 'Sample Completed', '2025-12-21 11:16:23.3909+08', '11', 'Fisher');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (306, 'ProductOrder', '704', 'TB649431', NULL, 'Manufacture', 'Manufacture Completed', '2025-12-21 11:16:38.573334+08', '9', 'Tuda');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (307, 'ProductOrder', '704', 'TB649431', NULL, 'QC check', 'QC check complted', '2025-12-21 11:16:59.514973+08', '13', 'Jimi');
INSERT INTO public.biz_app_flow (id, app_name, app_instance_id, app_instance_code, status, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (308, 'ProductOrder', '704', 'TB649431', NULL, 'Weight', 'Weight Completed', '2025-12-21 11:17:18.05419+08', '15', 'Damark');


--
-- Data for Name: fb_form; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.fb_form (id, form_id, form_title, version, field_summary, template_content, html_content, description, created_date, updated_date) VALUES (1, 'Form_MeetingRoomBooking', '会议室预定表单', '1', '["timeOptions","roomOptions","meetingSubject","reservationDate","timeSlot","roomSelection","participantsNumber","equipmentNeeds","remarks"]', '{"components":[{"label":"会议主题","key":"meetingSubject","type":"textfield","id":"Field_1","layout":{"row":"Row_1","columns":12}},{"label":"预定日期","key":"reservationDate","type":"textfield","id":"Field_2","layout":{"row":"Row_2","columns":12}},{"label":"时间段选择","key":"timeSlot","type":"select","id":"Field_3","valuesKey":"timeOptions","layout":{"row":"Row_3","columns":12}},{"label":"会议室选择","key":"roomSelection","type":"select","id":"Field_4","valuesKey":"roomOptions","layout":{"row":"Row_4","columns":12}},{"label":"参与人数","key":"participantsNumber","type":"number","id":"Field_5","layout":{"row":"Row_5","columns":12},"properties":{"key1":"value"}},{"label":"需要投影设备","key":"equipmentNeeds","type":"checkbox","id":"Field_6","layout":{"row":"Row_6","columns":12}},{"label":"备注说明","key":"remarks","type":"textarea","id":"Field_7","layout":{"row":"Row_7","columns":12}}],"schemaVersion":18,"exporter":{"name":"FormDesigner","version":"1.0.0"},"type":"default","id":"Form_MeetingRoomBooking"}', NULL, 'This form is craated by AI', '2025-09-03 10:22:52.531+08', '2025-09-03 10:24:06.938+08');


--
-- Data for Name: fb_form_field_event; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.fb_form_field_event (id, form_id, field_id, event_name, event_arguments, is_disabled, command_text) VALUES (12, 0, 0, 'onchange', NULL, 0, 'alert(''beijing'')');


--
-- Data for Name: fb_form_process; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.fb_form_process (id, form_id, version, process_def_id, process_version, process_id, process_name) VALUES (14, 24, '1', 3, '1', '072af8c3-482a-4b1c-890b-685ce2fcc75d', 'PriceProcess(SequenceTest)');
INSERT INTO public.fb_form_process (id, form_id, version, process_def_id, process_version, process_id, process_name) VALUES (15, 32, '1', 3, '1', '072af8c3-482a-4b1c-890b-685ce2fcc75d', 'PriceProcess(SequenceTest)');
INSERT INTO public.fb_form_process (id, form_id, version, process_def_id, process_version, process_id, process_name) VALUES (16, 33, '1', 3, '1', '072af8c3-482a-4b1c-890b-685ce2fcc75d', 'PriceProcess(SequenceTest)');
INSERT INTO public.fb_form_process (id, form_id, version, process_def_id, process_version, process_id, process_name) VALUES (17, 34, '1', 3, '1', '072af8c3-482a-4b1c-890b-685ce2fcc75d', 'PriceProcess(SequenceTest)');
INSERT INTO public.fb_form_process (id, form_id, version, process_def_id, process_version, process_id, process_name) VALUES (18, 35, '1', 3, '1', '072af8c3-482a-4b1c-890b-685ce2fcc75d', 'PriceProcess(SequenceTest)');
INSERT INTO public.fb_form_process (id, form_id, version, process_def_id, process_version, process_id, process_name) VALUES (19, 36, '2', 3, '1', '072af8c3-482a-4b1c-890b-685ce2fcc75d', 'PriceProcess(SequenceTest)');
INSERT INTO public.fb_form_process (id, form_id, version, process_def_id, process_version, process_id, process_name) VALUES (20, 39, '2', 3, '1', '072af8c3-482a-4b1c-890b-685ce2fcc75d', 'PriceProcess(SequenceTest)');
INSERT INTO public.fb_form_process (id, form_id, version, process_def_id, process_version, process_id, process_name) VALUES (21, 52, '1', 3, '1', '072af8c3-482a-4b1c-890b-685ce2fcc75d', 'PriceProcess(SequenceTest)');
INSERT INTO public.fb_form_process (id, form_id, version, process_def_id, process_version, process_id, process_name) VALUES (23, 56, '1', 24, '1', '2acffb20-6bd1-4891-98c9-c76d022d1445', '请假流程(WebDemo)');
INSERT INTO public.fb_form_process (id, form_id, version, process_def_id, process_version, process_id, process_name) VALUES (24, 57, '1', 3, '1', '072af8c3-482a-4b1c-890b-685ce2fcc75d', 'PriceProcess(SequenceTest)');
INSERT INTO public.fb_form_process (id, form_id, version, process_def_id, process_version, process_id, process_name) VALUES (25, 60, '1', 24, '1', '2acffb20-6bd1-4891-98c9-c76d022d1445', '请假流程(WebDemo)');
INSERT INTO public.fb_form_process (id, form_id, version, process_def_id, process_version, process_id, process_name) VALUES (26, 61, '1', 24, '1', '2acffb20-6bd1-4891-98c9-c76d022d1445', '请假流程(WebDemo)');
INSERT INTO public.fb_form_process (id, form_id, version, process_def_id, process_version, process_id, process_name) VALUES (27, 62, '1', 104, '1', 'b2a18777-43f1-4d4d-b9d5-f92aa655a93f', 'Ask for leave');
INSERT INTO public.fb_form_process (id, form_id, version, process_def_id, process_version, process_id, process_name) VALUES (32, 63, '1', 857, '1', '75BF39C8-5F4B-441F-9A08-39E0B46A9903', 'AskForLeave(WebDemo)');
INSERT INTO public.fb_form_process (id, form_id, version, process_def_id, process_version, process_id, process_name) VALUES (35, 65, '1', 1483, '1', 'fdad2829-7449-4840-b6f7-1104b19972d5', 'Process_Name_3023');


--
-- Data for Name: hrs_leave; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.hrs_leave (id, leave_type, days, from_date, to_date, current_activity_text, status, created_user_id, created_user_name, created_datetime, remark, opinions) VALUES (80, '事假', 4.0, '2021-04-04', '2021-04-08', NULL, 0, '6', 'Lucy', '2021-04-13 00:00:00+08', 'dsf', 'safewfewasf');
INSERT INTO public.hrs_leave (id, leave_type, days, from_date, to_date, current_activity_text, status, created_user_id, created_user_name, created_datetime, remark, opinions) VALUES (81, '事假', 4.0, '2021-04-03', '2021-04-07', NULL, 0, '6', 'Lucy', '2021-04-13 00:00:00+08', 'wfe', 'eqwrewqrfsdaegfeasfasfds');
INSERT INTO public.hrs_leave (id, leave_type, days, from_date, to_date, current_activity_text, status, created_user_id, created_user_name, created_datetime, remark, opinions) VALUES (82, '事假', 6.0, '2021-04-03', '2021-04-09', NULL, 0, '6', 'Lucy', '2021-04-13 00:00:00+08', 'asdf', 'wrdsadfsftrhgfr');
INSERT INTO public.hrs_leave (id, leave_type, days, from_date, to_date, current_activity_text, status, created_user_id, created_user_name, created_datetime, remark, opinions) VALUES (83, '事假', 3.0, '2021-04-04', '2021-04-07', NULL, 0, '6', 'Lucy', '2021-04-29 00:00:00+08', 'u', 'ydfyi');
INSERT INTO public.hrs_leave (id, leave_type, days, from_date, to_date, current_activity_text, status, created_user_id, created_user_name, created_datetime, remark, opinions) VALUES (84, 'Personal', 2.0, '2022-07-07', '2022-07-09', NULL, 0, '6', 'Lucy', '2022-07-08 00:00:00+08', 'test', '');
INSERT INTO public.hrs_leave (id, leave_type, days, from_date, to_date, current_activity_text, status, created_user_id, created_user_name, created_datetime, remark, opinions) VALUES (85, '事假', 2.0, '2025-02-23', '2025-02-25', NULL, 0, '6', 'Lucy', '2025-02-23 00:00:00+08', 'hello', '');
INSERT INTO public.hrs_leave (id, leave_type, days, from_date, to_date, current_activity_text, status, created_user_id, created_user_name, created_datetime, remark, opinions) VALUES (86, '事假', 3.0, '2025-09-02', '2025-09-05', NULL, 0, '6', 'Lucy', '2025-09-03 00:00:00+08', '', '同意');
INSERT INTO public.hrs_leave (id, leave_type, days, from_date, to_date, current_activity_text, status, created_user_id, created_user_name, created_datetime, remark, opinions) VALUES (87, '有薪假期', 6.0, '2025-09-02', '2025-09-08', NULL, 0, '6', 'Lucy', '2025-09-03 00:00:00+08', '', '');
INSERT INTO public.hrs_leave (id, leave_type, days, from_date, to_date, current_activity_text, status, created_user_id, created_user_name, created_datetime, remark, opinions) VALUES (88, 'Personal', 3, '2025-12-23', '2025-12-26', NULL, 0, '6', 'Lucy', '2025-12-21 11:33:48.100806+08', '', '');


--
-- Data for Name: hrs_leave_opinion; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.hrs_leave_opinion (id, app_instance_id, activity_id, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (1, '34', '00000000-0000-0000-0000-000000000000', '流程发起', '申请人:6-路天明', '2017-07-26 18:34:26.04+08', 6, '路天明');
INSERT INTO public.hrs_leave_opinion (id, app_instance_id, activity_id, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (2, '34', 'c437c27a-8351-4805-fd4f-4e270084320a', '部门经理审批', '张恒丰(ID:5) agree', '2017-07-26 18:35:32.293+08', 5, '张恒丰');
INSERT INTO public.hrs_leave_opinion (id, app_instance_id, activity_id, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (3, '35', '00000000-0000-0000-0000-000000000000', '流程发起', '申请人:6-路天明', '2017-08-23 19:07:59.453+08', 6, '路天明');
INSERT INTO public.hrs_leave_opinion (id, app_instance_id, activity_id, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (4, '35', 'c437c27a-8351-4805-fd4f-4e270084320a', '部门经理审批', '张恒丰(ID:5) tongyi', '2017-08-23 19:08:33.657+08', 5, '张恒丰');
INSERT INTO public.hrs_leave_opinion (id, app_instance_id, activity_id, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (5, '36', '00000000-0000-0000-0000-000000000000', '流程发起', '申请人:6-路天明', '2017-09-14 10:42:52.79+08', 6, '路天明');
INSERT INTO public.hrs_leave_opinion (id, app_instance_id, activity_id, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (6, '37', '00000000-0000-0000-0000-000000000000', '流程发起', '申请人:6-路天明', '2017-12-04 14:01:59.923+08', 6, '路天明');
INSERT INTO public.hrs_leave_opinion (id, app_instance_id, activity_id, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (7, '37', 'c437c27a-8351-4805-fd4f-4e270084320a', '部门经理审批', '张恒丰(ID:5) 同意', '2017-12-04 14:02:40.56+08', 5, '张恒丰');
INSERT INTO public.hrs_leave_opinion (id, app_instance_id, activity_id, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (8, '37', 'da9f744b-3f97-40c9-c4f8-67d5a60a2485', '人事经理审批', '李颖(ID:4) ', '2017-12-04 14:03:46.973+08', 4, '李颖');
INSERT INTO public.hrs_leave_opinion (id, app_instance_id, activity_id, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (9, '38', '00000000-0000-0000-0000-000000000000', '流程发起', '申请人:6-路天明', '2017-12-07 16:19:09.087+08', 6, '路天明');
INSERT INTO public.hrs_leave_opinion (id, app_instance_id, activity_id, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (10, '39', '00000000-0000-0000-0000-000000000000', '流程发起', '申请人:6-LuTianMing', '2020-08-26 15:27:28.673+08', 6, 'LuTianMing');
INSERT INTO public.hrs_leave_opinion (id, app_instance_id, activity_id, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (11, '39', 'c437c27a-8351-4805-fd4f-4e270084320a', '部门经理审批', 'ZhangFeng(ID:5) 同意', '2020-08-26 15:29:57.867+08', 5, 'ZhangFeng');
INSERT INTO public.hrs_leave_opinion (id, app_instance_id, activity_id, activity_name, remark, changed_time, changed_user_id, changed_user_name) VALUES (12, '39', 'da9f744b-3f97-40c9-c4f8-67d5a60a2485', '人事经理审批', 'LiYin(ID:4) ', '2020-08-26 15:30:23.47+08', 4, 'LiYin');


--
-- Data for Name: man_product_order; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (675, 'TB324384', 8, '遥控灯D型', 5, 1000.00, 5000.00, '2017-03-01 15:03:58.823+08', 'BBC', '英国伦敦', '739538', 'C店', '2017-03-01 15:34:05.057+08');
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (676, 'TB377329', 3, '遥控灯D型', 7, 1000.00, 7000.00, '2017-06-12 11:56:23.597+08', '阿里巴巴', '杭州西湖区', '802382', 'B店', '2017-06-12 12:26:45.683+08');
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (677, 'TB730548', 3, '智能玩具C型', 6, 1000.00, 6000.00, '2017-06-13 09:50:28.3+08', '汇丰银行', '上海人民广场', '338600', 'F店', '2018-06-17 16:56:33.227+08');
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (678, 'TB574787', 3, '智能玩具C型', 7, 1000.00, 7000.00, '2017-07-22 09:36:06.88+08', '汇丰银行', '上海人民广场', '553578', 'C店', '2017-07-22 09:36:25.913+08');
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (679, 'TB100834', 4, '童话玩具A型', 6, 1000.00, 6000.00, '2017-08-23 19:06:50.267+08', 'HACK 新闻', '美国纽约', '974724', 'A店', '2017-08-23 19:07:21.627+08');
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (680, 'TB752624', 8, '海盗船F型', 4, 1000.00, 4000.00, '2017-12-04 11:05:08.47+08', '花旗银行', '上海浦东新区', '100628', 'F店', '2017-12-04 11:07:26.287+08');
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (681, 'TB517477', 3, '童话玩具A型', 4, 1000.00, 4000.00, '2017-12-04 13:56:31.4+08', '中石油', '北京燕山', '120409', 'C店', '2017-12-07 16:13:31.437+08');
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (682, 'TB601588', 4, '遥控灯D型', 4, 1000.00, 4000.00, '2017-12-07 16:14:04.323+08', '花旗银行', '上海浦东新区', '428885', 'A店', '2017-12-07 16:14:26.527+08');
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (683, 'TB393078', 3, 'LED节能灯E型', 1, 1000.00, 1000.00, '2018-10-19 10:46:09.983+08', '阿里巴巴', '杭州西湖区', '500282', 'B店', '2022-03-22 10:27:34.883+08');
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (684, 'TB937073', 3, '智能玩具C型', 1, 1000.00, 1000.00, '2019-03-06 21:21:41.707+08', '中石油', '北京燕山', '376673', 'F店', '2019-03-06 21:22:34.097+08');
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (685, 'TB359987', 3, '海盗船F型', 9, 1000.00, 9000.00, '2019-10-30 14:31:05.917+08', '中国邮政', '北京复兴门', '568964', 'F店', '2019-10-30 14:31:17.383+08');
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (686, 'TB588656', 8, '智能玩具C型', 3, 1000.00, 3000.00, '2020-05-15 10:28:54.677+08', '花旗银行', '上海浦东新区', '666540', 'B店', '2020-05-15 10:54:05.273+08');
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (687, 'TB720748', 8, '遥控飞机B型', 4, 1000.00, 4000.00, '2020-08-26 14:41:11.37+08', '花旗银行', '上海浦东新区', '140223', 'C店', '2020-08-26 15:17:36.503+08');
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (688, 'TB332639', 3, '童话玩具A型', 1, 1000.00, 1000.00, '2020-11-25 12:35:53.207+08', '阿里巴巴', '杭州西湖区', '175105', 'B店', '2021-01-14 09:43:25.573+08');
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (689, 'TB741954', 8, '童话玩具A型', 4, 1000.00, 4000.00, '2021-01-14 09:49:17.65+08', '中石油', '北京燕山', '164151', 'B店', '2021-01-14 09:50:48.747+08');
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (690, 'TB332806', 3, '童话玩具A型', 2, 1000.00, 2000.00, '2021-08-25 15:33:01.68+08', '青田麦家', '福建岭南', '909976', 'C店', '2021-08-25 15:33:20.367+08');
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (691, 'TB452818', 3, '遥控灯D型', 2, 1000.00, 2000.00, '2022-03-22 10:27:49.133+08', '中石油', '北京燕山', '534659', 'F店', '2022-03-22 10:28:11.997+08');
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (692, 'TB263682', 8, '海盗船F型', 7, 1000.00, 7000.00, '2022-03-22 18:18:43.567+08', '汇丰银行', '上海人民广场', '636622', 'A店', '2022-07-09 08:42:04.943+08');
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (693, 'TB307954', 8, '童话玩具A型', 1, 1000.00, 1000.00, '2022-04-25 14:36:38.467+08', '阿里巴巴', '杭州西湖区', '248689', 'B店', '2022-04-25 14:38:34.857+08');
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (694, 'TB293064', 3, '智能玩具C型', 6, 1000.00, 6000.00, '2022-07-10 20:00:54.687+08', '阿里巴巴', '杭州西湖区', '960191', 'J店', '2022-07-10 20:01:42.857+08');
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (695, 'TB226532', 3, '遥控飞机B型', 6, 1000.00, 6000.00, '2024-02-04 16:02:33.66+08', '青田麦家', '福建岭南', '675404', 'A店', '2024-02-04 16:02:52.853+08');
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (696, 'TB207310', 8, '智能玩具C型', 1, 1000.00, 1000.00, '2024-06-21 09:16:00.55+08', '中石油', '北京燕山', '558283', 'F店', '2024-06-21 10:24:42.91+08');
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (697, 'TB257249', 4, '童话玩具A型', 1, 1000.00, 1000.00, '2024-06-21 11:00:20.997+08', '汇丰银行', '上海人民广场', '678931', 'A店', '2024-06-21 11:01:09.443+08');
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (698, 'TB409086', 4, '遥控飞机B型', 3, 1000.00, 3000.00, '2024-06-21 13:45:59.173+08', 'BBC', '英国伦敦', '653688', 'J店', '2024-06-21 13:46:53.493+08');
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (699, 'TB849499', 8, 'Aircraft-B', 1, 1000.00, 1000.00, '2025-02-22 20:04:38.497+08', 'MaiJIA', 'Lingnan', '245143', 'Store-A', '2025-02-22 20:54:12.563+08');
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (700, 'TB751527', 8, 'SmartToy-C', 2, 1000.00, 2000.00, '2025-09-03 08:36:20.888+08', 'UBS', 'Wangfujing', '463620', 'Store-J', '2025-09-03 08:39:49.055+08');
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (701, 'TB967640', 1, 'LED-D', 6, 1000, 6000, '2025-12-21 18:34:02.44188+08', 'PetrolShell', 'Yanshan', '106129', 'Store-J', NULL);
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (702, 'TB646364', 1, 'FairyTale-A', 6, 1000, 6000, '2025-12-21 18:34:30.091166+08', 'MaiJIA', 'Lingnan', '700300', 'Store-A', NULL);
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (703, 'TB174709', 1, 'PirateShip-F', 2, 1000, 2000, '2025-12-21 18:36:24.217617+08', 'UBS', 'Wangfujing', '379748', 'Store-F', NULL);
INSERT INTO public.man_product_order (id, order_code, status, product_name, quantity, unit_price, total_price, created_time, customer_name, address, mobile, remark, updated_time) VALUES (704, 'TB649431', 8, 'FairyTale-A', 7, 1000, 7000, '2025-12-21 18:57:49.13416+08', 'PostUK', 'FuxingGate', '529786', 'Store-F', '2025-12-21 11:17:18.059934+08');


--
-- Data for Name: sf_portal_tenant; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.sf_portal_tenant (id, email, display_name, plan, created_at) VALUES ('test-user-001', 'testuser@example.com', NULL, 'free', '2026-06-14 21:03:33.548113+08');
INSERT INTO public.sf_portal_tenant (id, email, display_name, plan, created_at) VALUES ('3a7d68a0-7f7f-4270-aa3c-8480ed96e3ce', 'besley@163.com', NULL, 'free', '2026-06-17 08:56:41.570954+08');


--
-- Data for Name: sf_portal_quota; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.sf_portal_quota (tenant_id, period_start, process_instances_used, ai_calls_used, custom_processes_used) VALUES ('test-user-001', '2026-06-01', 1, 0, 0);
INSERT INTO public.sf_portal_quota (tenant_id, period_start, process_instances_used, ai_calls_used, custom_processes_used) VALUES ('3a7d68a0-7f7f-4270-aa3c-8480ed96e3ce', '2026-06-01', 5, 0, 0);


--
-- Data for Name: sys_department; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.sys_department (id, dept_code, dept_name, parent_dept_id, description) VALUES (1, 'CP', 'SlickOne科技', 0, NULL);
INSERT INTO public.sys_department (id, dept_code, dept_name, parent_dept_id, description) VALUES (2, 'TH', '技术部', 1, NULL);
INSERT INTO public.sys_department (id, dept_code, dept_name, parent_dept_id, description) VALUES (3, 'HR', '人事部', 1, NULL);
INSERT INTO public.sys_department (id, dept_code, dept_name, parent_dept_id, description) VALUES (4, 'FN', '财务部', 1, NULL);


--
-- Data for Name: sys_employee; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.sys_employee (id, dept_id, emp_code, emp_name, user_id, mobile, email, remark) VALUES (1, 2, '0001', '路天明', 6, NULL, NULL, NULL);
INSERT INTO public.sys_employee (id, dept_id, emp_code, emp_name, user_id, mobile, email, remark) VALUES (2, 2, '0002', '张经理', 5, NULL, NULL, NULL);
INSERT INTO public.sys_employee (id, dept_id, emp_code, emp_name, user_id, mobile, email, remark) VALUES (3, 3, '0003', '金经理', 18, NULL, NULL, NULL);
INSERT INTO public.sys_employee (id, dept_id, emp_code, emp_name, user_id, mobile, email, remark) VALUES (4, 4, '0004', '阿杰', 10, NULL, NULL, NULL);
INSERT INTO public.sys_employee (id, dept_id, emp_code, emp_name, user_id, mobile, email, remark) VALUES (5, 4, '0005', '崔经理', 17, NULL, NULL, NULL);
INSERT INTO public.sys_employee (id, dept_id, emp_code, emp_name, user_id, mobile, email, remark) VALUES (6, 2, '0010', '张明', 9, NULL, NULL, NULL);
INSERT INTO public.sys_employee (id, dept_id, emp_code, emp_name, user_id, mobile, email, remark) VALUES (7, 4, '0030', '金兰', 18, NULL, NULL, NULL);


--
-- Data for Name: sys_employee_manager; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.sys_employee_manager (id, employee_id, employee_user_id, manager_id, manager_user_id) VALUES (1, 1, 6, 2, 5);
INSERT INTO public.sys_employee_manager (id, employee_id, employee_user_id, manager_id, manager_user_id) VALUES (2, 4, 10, 5, 17);
INSERT INTO public.sys_employee_manager (id, employee_id, employee_user_id, manager_id, manager_user_id) VALUES (4, 6, 9, 3, 5);
INSERT INTO public.sys_employee_manager (id, employee_id, employee_user_id, manager_id, manager_user_id) VALUES (5, 4, 10, 7, 18);


--
-- Data for Name: sys_resource; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.sys_resource (id, resource_type, parent_resource_id, resource_name, resource_code, order_number) VALUES (1, 1, 0, '生产订单系统', 'SfDemo.Made', 1);
INSERT INTO public.sys_resource (id, resource_type, parent_resource_id, resource_name, resource_code, order_number) VALUES (2, 2, 1, '生产订单流程', 'SfDemo.Made.POrder', 1);
INSERT INTO public.sys_resource (id, resource_type, parent_resource_id, resource_name, resource_code, order_number) VALUES (4, 5, 2, '同步订单', 'SfDemo.Made.POrder.SyncOrder', 1);
INSERT INTO public.sys_resource (id, resource_type, parent_resource_id, resource_name, resource_code, order_number) VALUES (5, 5, 2, '分派订单', 'SfDemo.Made.POrder.Dispatch', 2);
INSERT INTO public.sys_resource (id, resource_type, parent_resource_id, resource_name, resource_code, order_number) VALUES (6, 5, 2, '打样', 'SfDemo.Made.POrder.Sample', 3);
INSERT INTO public.sys_resource (id, resource_type, parent_resource_id, resource_name, resource_code, order_number) VALUES (7, 5, 2, '生产', 'SfDemo.Made.POrder.Manufacture', 4);
INSERT INTO public.sys_resource (id, resource_type, parent_resource_id, resource_name, resource_code, order_number) VALUES (8, 5, 2, '质检', 'SfDemo.Made.POrder.QCCheck', 5);
INSERT INTO public.sys_resource (id, resource_type, parent_resource_id, resource_name, resource_code, order_number) VALUES (9, 5, 2, '称重', 'SfDemo.Made.POrder.Weight', 6);
INSERT INTO public.sys_resource (id, resource_type, parent_resource_id, resource_name, resource_code, order_number) VALUES (10, 5, 2, '发货', 'SfDemo.Made.POrder.Delivery', 7);


--
-- Data for Name: sys_role; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.sys_role (id, role_code, role_name) VALUES (1, 'employees', '普通员工');
INSERT INTO public.sys_role (id, role_code, role_name) VALUES (2, 'depmanager', '部门经理');
INSERT INTO public.sys_role (id, role_code, role_name) VALUES (3, 'hrmanager', '人事经理');
INSERT INTO public.sys_role (id, role_code, role_name) VALUES (4, 'director', '主管总监');
INSERT INTO public.sys_role (id, role_code, role_name) VALUES (7, 'deputygeneralmanager', '副总经理');
INSERT INTO public.sys_role (id, role_code, role_name) VALUES (8, 'generalmanager', '总经理');
INSERT INTO public.sys_role (id, role_code, role_name) VALUES (9, 'salesmate', '业务员salesmate');
INSERT INTO public.sys_role (id, role_code, role_name) VALUES (10, 'techmate', '打样员techmate');
INSERT INTO public.sys_role (id, role_code, role_name) VALUES (11, 'merchandiser', '跟单员merchandiser');
INSERT INTO public.sys_role (id, role_code, role_name) VALUES (12, 'qcmate', '质检员qcmate');
INSERT INTO public.sys_role (id, role_code, role_name) VALUES (13, 'expressmate', '包装员expressmate');
INSERT INTO public.sys_role (id, role_code, role_name) VALUES (14, 'finacemanager', '财务经理');
INSERT INTO public.sys_role (id, role_code, role_name) VALUES (21, 'testrole', 'testrole');


--
-- Data for Name: sys_role_group_resource; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.sys_role_group_resource (id, role_group_type, role_group_id, resource_id, permission_type) VALUES (1, 1, 9, 1, 1);
INSERT INTO public.sys_role_group_resource (id, role_group_type, role_group_id, resource_id, permission_type) VALUES (2, 1, 9, 2, 1);
INSERT INTO public.sys_role_group_resource (id, role_group_type, role_group_id, resource_id, permission_type) VALUES (3, 1, 9, 4, 1);
INSERT INTO public.sys_role_group_resource (id, role_group_type, role_group_id, resource_id, permission_type) VALUES (4, 1, 9, 5, 1);
INSERT INTO public.sys_role_group_resource (id, role_group_type, role_group_id, resource_id, permission_type) VALUES (5, 1, 10, 1, 1);
INSERT INTO public.sys_role_group_resource (id, role_group_type, role_group_id, resource_id, permission_type) VALUES (6, 1, 10, 2, 1);
INSERT INTO public.sys_role_group_resource (id, role_group_type, role_group_id, resource_id, permission_type) VALUES (7, 1, 10, 6, 1);
INSERT INTO public.sys_role_group_resource (id, role_group_type, role_group_id, resource_id, permission_type) VALUES (8, 1, 11, 7, 1);
INSERT INTO public.sys_role_group_resource (id, role_group_type, role_group_id, resource_id, permission_type) VALUES (9, 1, 12, 8, 1);
INSERT INTO public.sys_role_group_resource (id, role_group_type, role_group_id, resource_id, permission_type) VALUES (10, 1, 13, 9, 1);
INSERT INTO public.sys_role_group_resource (id, role_group_type, role_group_id, resource_id, permission_type) VALUES (11, 1, 13, 10, 1);


--
-- Data for Name: sys_role_user; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.sys_role_user (id, role_id, user_id) VALUES (1, 8, 1);
INSERT INTO public.sys_role_user (id, role_id, user_id) VALUES (2, 7, 2);
INSERT INTO public.sys_role_user (id, role_id, user_id) VALUES (3, 4, 3);
INSERT INTO public.sys_role_user (id, role_id, user_id) VALUES (4, 3, 4);
INSERT INTO public.sys_role_user (id, role_id, user_id) VALUES (5, 2, 5);
INSERT INTO public.sys_role_user (id, role_id, user_id) VALUES (6, 1, 6);
INSERT INTO public.sys_role_user (id, role_id, user_id) VALUES (7, 9, 7);
INSERT INTO public.sys_role_user (id, role_id, user_id) VALUES (8, 9, 8);
INSERT INTO public.sys_role_user (id, role_id, user_id) VALUES (9, 10, 11);
INSERT INTO public.sys_role_user (id, role_id, user_id) VALUES (10, 10, 12);
INSERT INTO public.sys_role_user (id, role_id, user_id) VALUES (11, 11, 9);
INSERT INTO public.sys_role_user (id, role_id, user_id) VALUES (12, 11, 10);
INSERT INTO public.sys_role_user (id, role_id, user_id) VALUES (13, 12, 13);
INSERT INTO public.sys_role_user (id, role_id, user_id) VALUES (14, 12, 14);
INSERT INTO public.sys_role_user (id, role_id, user_id) VALUES (15, 13, 15);
INSERT INTO public.sys_role_user (id, role_id, user_id) VALUES (16, 13, 16);
INSERT INTO public.sys_role_user (id, role_id, user_id) VALUES (17, 14, 17);
INSERT INTO public.sys_role_user (id, role_id, user_id) VALUES (19, 2, 17);


--
-- Data for Name: sys_user; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.sys_user (id, user_name, email) VALUES (1, 'Cindy', NULL);
INSERT INTO public.sys_user (id, user_name, email) VALUES (2, 'Henry', NULL);
INSERT INTO public.sys_user (id, user_name, email) VALUES (3, 'Test', 'jack@163.com');
INSERT INTO public.sys_user (id, user_name, email) VALUES (4, 'LeeO', NULL);
INSERT INTO public.sys_user (id, user_name, email) VALUES (5, 'Ada', NULL);
INSERT INTO public.sys_user (id, user_name, email) VALUES (6, 'Lucy', NULL);
INSERT INTO public.sys_user (id, user_name, email) VALUES (7, 'Peter', NULL);
INSERT INTO public.sys_user (id, user_name, email) VALUES (8, 'Bill', NULL);
INSERT INTO public.sys_user (id, user_name, email) VALUES (9, 'Tuda', NULL);
INSERT INTO public.sys_user (id, user_name, email) VALUES (10, 'Jack', 'hr@ruochisoft.com');
INSERT INTO public.sys_user (id, user_name, email) VALUES (11, 'Fisher', 'hr@ruochisoft.com');
INSERT INTO public.sys_user (id, user_name, email) VALUES (12, 'Sherley', 'hr@ruochisoft.com');
INSERT INTO public.sys_user (id, user_name, email) VALUES (13, 'Jimi', NULL);
INSERT INTO public.sys_user (id, user_name, email) VALUES (14, 'William', NULL);
INSERT INTO public.sys_user (id, user_name, email) VALUES (15, 'Damark', NULL);
INSERT INTO public.sys_user (id, user_name, email) VALUES (16, 'Smith', NULL);
INSERT INTO public.sys_user (id, user_name, email) VALUES (17, 'Yolanda', NULL);
INSERT INTO public.sys_user (id, user_name, email) VALUES (18, 'Jinny', NULL);
INSERT INTO public.sys_user (id, user_name, email) VALUES (19, 'Susan', 'hr@ruochisoft.com');


--
-- Data for Name: sys_user_resource; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (1, 7, 1);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (2, 7, 2);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (3, 7, 4);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (4, 7, 5);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (5, 8, 1);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (6, 8, 2);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (7, 8, 4);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (8, 8, 5);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (9, 11, 1);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (10, 11, 2);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (11, 11, 6);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (12, 12, 1);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (13, 12, 2);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (14, 12, 6);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (15, 9, 1);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (16, 9, 2);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (17, 9, 7);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (18, 10, 1);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (19, 10, 2);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (20, 10, 7);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (21, 13, 1);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (22, 13, 2);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (23, 13, 8);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (24, 14, 1);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (25, 14, 2);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (26, 14, 8);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (27, 15, 1);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (28, 15, 2);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (29, 15, 9);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (30, 15, 10);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (31, 16, 1);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (32, 16, 2);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (33, 16, 9);
INSERT INTO public.sys_user_resource (id, user_id, resource_id) VALUES (34, 16, 10);


--
-- Data for Name: t_inquiry; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.t_inquiry (id, inquiry_no, company_name, contact_name, email, phone, product_category, project_description, quantity, target_price, delivery_date, status, priority, estimated_value, received_at, processed_at, workflow_instance_id) VALUES ('f78d88d3-73f2-4a64-9f4b-c87e1a73cb46', 'INQ-20260603-001', 'yyy', 'jack', 'jack@43545.com', '43590', 'LED Panel Light', 'khk', 19, NULL, NULL, 4, 2, 0.00, '2026-06-03 08:21:50.120809+08', NULL, '1801');
INSERT INTO public.t_inquiry (id, inquiry_no, company_name, contact_name, email, phone, product_category, project_description, quantity, target_price, delivery_date, status, priority, estimated_value, received_at, processed_at, workflow_instance_id) VALUES ('c8e3be71-4f30-476f-8932-9501cd33a128', 'INQ-20260603-002', 'aaa', 'jcakkdf', 'jack@1253.com', '340985097', 'LED Canopy Light', 'lkh', 28, NULL, NULL, 4, 2, 0.00, '2026-06-03 14:49:50.516505+08', NULL, '1802');
INSERT INTO public.t_inquiry (id, inquiry_no, company_name, contact_name, email, phone, product_category, project_description, quantity, target_price, delivery_date, status, priority, estimated_value, received_at, processed_at, workflow_instance_id) VALUES ('2afd9918-5b2c-4c10-8297-7c5246fdeb1c', 'INQ-20260602-001', 'Walmart Distribution Center', 'Mike Johnson', 'mjohnson@walmart.com', '+1-555-2024', 'LED High Bay', 'Warehouse lighting upgrade: 200W LED High Bay, IP65, DLC certified, 10m ceiling height, 500lux requirement', 300, NULL, NULL, 1, 0, 0.00, '2026-06-02 21:19:08.74573+08', NULL, '1796');
INSERT INTO public.t_inquiry (id, inquiry_no, company_name, contact_name, email, phone, product_category, project_description, quantity, target_price, delivery_date, status, priority, estimated_value, received_at, processed_at, workflow_instance_id) VALUES ('aee84a85-ab74-4a0c-bef6-60a8c8acd9e5', 'INQ-20260602-002', 'Home Depot Pro', 'Sarah Chen', 'schen@homedepot.com', '+1-555-3388', 'LED Flood Light', 'Outdoor parking lot lighting: 150W LED Flood Light, IP67, 5000K, UL listed', 120, NULL, NULL, 1, 1, 0.00, '2026-06-02 21:20:39.934619+08', NULL, '1797');


--
-- Data for Name: wf_process; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (9, 'Process_0yyy_7671', '1', 'Sequence_7671', 'Sequence_Code_7671', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_0yyy_7671" sf:code="Sequence_Code_7671" name="Sequence_7671" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="StartNode_8644" sf:code="Start" name="Start">
      <bpmn:outgoing>Flow_7207</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:task id="TaskNode_2003" sf:code="task001" name="Task-001">
      <bpmn:incoming>Flow_7207</bpmn:incoming>
      <bpmn:outgoing>Flow_8026</bpmn:outgoing>
    </bpmn:task>
    <bpmn:task id="TaskNode_9006" sf:code="task002" name="Task-002">
      <bpmn:incoming>Flow_8026</bpmn:incoming>
      <bpmn:outgoing>Flow_3496</bpmn:outgoing>
    </bpmn:task>
    <bpmn:task id="TaskNode_7873" sf:code="task003" name="Task-007">
      <bpmn:incoming>Flow_3496</bpmn:incoming>
      <bpmn:outgoing>Flow_2982</bpmn:outgoing>
    </bpmn:task>
    <bpmn:endEvent id="EndNode_4734" sf:code="End" name="End">
      <bpmn:incoming>Flow_2982</bpmn:incoming>
    </bpmn:endEvent>
    <bpmn:sequenceFlow id="Flow_7207" name="" sourceRef="StartNode_8644" targetRef="TaskNode_2003" />
    <bpmn:sequenceFlow id="Flow_8026" name="t-001" sourceRef="TaskNode_2003" targetRef="TaskNode_9006" />
    <bpmn:sequenceFlow id="Flow_3496" name="" sourceRef="TaskNode_9006" targetRef="TaskNode_7873" />
    <bpmn:sequenceFlow id="Flow_2982" name="" sourceRef="TaskNode_7873" targetRef="EndNode_4734" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_0yyy_7671">
      <bpmndi:BPMNShape id="BPMNShape_bfah2vh_di" bpmnElement="StartNode_8644">
        <dc:Bounds x="240" y="180" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_ckftu2v_di" bpmnElement="TaskNode_2003">
        <dc:Bounds x="356" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_qd7l03s_di" bpmnElement="TaskNode_9006">
        <dc:Bounds x="536" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_dizllfn_di" bpmnElement="TaskNode_7873">
        <dc:Bounds x="716" y="158" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_lkqla1p_di" bpmnElement="EndNode_4734">
        <dc:Bounds x="896" y="180" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_7207_di" bpmnElement="Flow_7207">
        <di:waypoint x="276" y="198" />
        <di:waypoint x="356" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_8026_di" bpmnElement="Flow_8026">
        <di:waypoint x="456" y="198" />
        <di:waypoint x="536" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_3496_di" bpmnElement="Flow_3496">
        <di:waypoint x="636" y="198" />
        <di:waypoint x="716" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_2982_di" bpmnElement="Flow_2982">
        <di:waypoint x="816" y="198" />
        <di:waypoint x="896" y="198" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  ', 0, NULL, 0, NULL, 'fas fa-envelope', NULL, '2025-09-02 12:46:05+08', '2026-01-03 22:58:33.816626+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (10, 'Process_s04w_6298', '1', 'Gateway_6298', 'Gateway_Code_6298', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_s04w_6298" sf:code="Gateway_Code_6298" name="Gateway_6298" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="StartNode_1494" sf:code="Start" name="start">
      <bpmn:outgoing>Flow_1518</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:task id="TaskNode_7687" sf:code="task001" name="Task-001">
      <bpmn:incoming>Flow_1518</bpmn:incoming>
      <bpmn:outgoing>Flow_7283</bpmn:outgoing>
    </bpmn:task>
    <bpmn:parallelGateway id="GatewayNode_9077" sf:code="andsplit001" name="and-split">
      <bpmn:incoming>Flow_7283</bpmn:incoming>
      <bpmn:outgoing>Flow_7663</bpmn:outgoing>
      <bpmn:outgoing>Flow_6234</bpmn:outgoing>
    </bpmn:parallelGateway>
    <bpmn:task id="TaskNode_7066" sf:code="task010" name="task-010">
      <bpmn:incoming>Flow_7663</bpmn:incoming>
      <bpmn:outgoing>Flow_4241</bpmn:outgoing>
    </bpmn:task>
    <bpmn:task id="TaskNode_1520" sf:code="task020" name="task-020">
      <bpmn:incoming>Flow_6234</bpmn:incoming>
      <bpmn:outgoing>Flow_4976</bpmn:outgoing>
    </bpmn:task>
    <bpmn:parallelGateway id="GatewayNode_8491" sf:code="andjoin001" name="and-join">
      <bpmn:incoming>Flow_4976</bpmn:incoming>
      <bpmn:incoming>Flow_4241</bpmn:incoming>
      <bpmn:outgoing>Flow_2215</bpmn:outgoing>
    </bpmn:parallelGateway>
    <bpmn:task id="TaskNode_7289" sf:code="task100" name="task-100">
      <bpmn:incoming>Flow_2215</bpmn:incoming>
      <bpmn:outgoing>Flow_2274</bpmn:outgoing>
    </bpmn:task>
    <bpmn:endEvent id="EndNode_1447" sf:code="End" name="end">
      <bpmn:incoming>Flow_2274</bpmn:incoming>
    </bpmn:endEvent>
    <bpmn:sequenceFlow id="Flow_1518" name="" sourceRef="StartNode_1494" targetRef="TaskNode_7687" />
    <bpmn:sequenceFlow id="Flow_7283" name="" sourceRef="TaskNode_7687" targetRef="GatewayNode_9077" />
    <bpmn:sequenceFlow id="Flow_7663" name="" sourceRef="GatewayNode_9077" targetRef="TaskNode_7066" />
    <bpmn:sequenceFlow id="Flow_6234" name="" sourceRef="GatewayNode_9077" targetRef="TaskNode_1520" />
    <bpmn:sequenceFlow id="Flow_4976" name="" sourceRef="TaskNode_1520" targetRef="GatewayNode_8491" />
    <bpmn:sequenceFlow id="Flow_4241" name="" sourceRef="TaskNode_7066" targetRef="GatewayNode_8491" />
    <bpmn:sequenceFlow id="Flow_2215" name="" sourceRef="GatewayNode_8491" targetRef="TaskNode_7289" />
    <bpmn:sequenceFlow id="Flow_2274" name="" sourceRef="TaskNode_7289" targetRef="EndNode_1447" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_s04w_6298">
      <bpmndi:BPMNShape id="BPMNShape_yb8ot9c_di" bpmnElement="StartNode_1494">
        <dc:Bounds x="240" y="180" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_og85zd3_di" bpmnElement="TaskNode_7687">
        <dc:Bounds x="356" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_ii842gq_di" bpmnElement="GatewayNode_9077">
        <dc:Bounds x="536" y="180" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="582" y="191" width="42" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_8jsokwd_di" bpmnElement="TaskNode_7066">
        <dc:Bounds x="652" y="230" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_i0xaunx_di" bpmnElement="TaskNode_1520">
        <dc:Bounds x="652" y="70" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_tc7gcaz_di" bpmnElement="GatewayNode_8491">
        <dc:Bounds x="832" y="180" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_ffvisgx_di" bpmnElement="TaskNode_7289">
        <dc:Bounds x="948" y="158" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="BPMNShape_kepd6hz_di" bpmnElement="EndNode_1447">
        <dc:Bounds x="1128" y="180" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_1518_di" bpmnElement="Flow_1518">
        <di:waypoint x="276" y="198" />
        <di:waypoint x="356" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_7283_di" bpmnElement="Flow_7283">
        <di:waypoint x="456" y="198" />
        <di:waypoint x="536" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_7663_di" bpmnElement="Flow_7663">
        <di:waypoint x="554" y="216" />
        <di:waypoint x="554" y="270" />
        <di:waypoint x="652" y="270" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_6234_di" bpmnElement="Flow_6234">
        <di:waypoint x="554" y="180" />
        <di:waypoint x="554" y="110" />
        <di:waypoint x="652" y="110" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_4976_di" bpmnElement="Flow_4976">
        <di:waypoint x="752" y="110" />
        <di:waypoint x="850" y="110" />
        <di:waypoint x="850" y="180" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_4241_di" bpmnElement="Flow_4241">
        <di:waypoint x="752" y="270" />
        <di:waypoint x="850" y="270" />
        <di:waypoint x="850" y="216" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_2215_di" bpmnElement="Flow_2215">
        <di:waypoint x="868" y="198" />
        <di:waypoint x="948" y="198" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_2274_di" bpmnElement="Flow_2274">
        <di:waypoint x="1048" y="198" />
        <di:waypoint x="1128" y="198" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
', 0, NULL, 0, NULL, 'fas fa-database', NULL, '2025-09-02 21:10:37+08', '2025-09-02 21:10:47.414+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (13, 'Process_m8nm_4177', '1', 'Contract_4177', 'Contract_Code_4177', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="utf-8"?><bpmn:definitions targetNamespace="http://bpmn.io/schema/bpmn" id="bpmn-diagram" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="Process_m8nm_4177" name="Contract_4177" isExecutable="true" sf:code="Contract_Code_4177" sf:version="1"><bpmn:startEvent id="StartNode_2236" name="start" sf:code="Start"><bpmn:outgoing>Flow_2449</bpmn:outgoing></bpmn:startEvent><bpmn:task id="TaskNode_8386" name="Contract Draft" sf:code="task001"><bpmn:incoming>Flow_2449</bpmn:incoming><bpmn:outgoing>Flow_7573</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_9243" name="Approved by BA Manager" sf:code="task002"><bpmn:incoming>Flow_7573</bpmn:incoming><bpmn:outgoing>Flow_3348</bpmn:outgoing></bpmn:task><bpmn:parallelGateway id="GatewayNode_9150" name="And-Split" sf:code="andsplit001"><bpmn:incoming>Flow_3348</bpmn:incoming><bpmn:outgoing>Flow_3521</bpmn:outgoing><bpmn:outgoing>Flow_4813</bpmn:outgoing><bpmn:outgoing>Flow_7760</bpmn:outgoing></bpmn:parallelGateway><bpmn:task id="TaskNode_5724" name="Contract Department Review" sf:code="task010"><bpmn:incoming>Flow_3521</bpmn:incoming><bpmn:outgoing>Flow_7152</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_8201" name="Financial Department Review" sf:code="task020"><bpmn:incoming>Flow_4813</bpmn:incoming><bpmn:outgoing>Flow_3952</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_3143" name="Group Headquarters Review" sf:code="task030"><bpmn:incoming>Flow_7760</bpmn:incoming><bpmn:outgoing>Flow_2130</bpmn:outgoing></bpmn:task><bpmn:parallelGateway id="GatewayNode_3367" name="And-Join" sf:code="andjoin001"><bpmn:incoming>Flow_2130</bpmn:incoming><bpmn:incoming>Flow_3952</bpmn:incoming><bpmn:incoming>Flow_7152</bpmn:incoming><bpmn:outgoing>Flow_9664</bpmn:outgoing></bpmn:parallelGateway><bpmn:task id="TaskNode_4815" name="Contract Archived" sf:code="task007"><bpmn:incoming>Flow_9664</bpmn:incoming><bpmn:outgoing>Flow_6958</bpmn:outgoing></bpmn:task><bpmn:endEvent id="EndNode_3956" name="end" sf:code="End"><bpmn:incoming>Flow_6958</bpmn:incoming></bpmn:endEvent><bpmn:sequenceFlow id="Flow_2449" name="" sourceRef="StartNode_2236" targetRef="TaskNode_8386" /><bpmn:sequenceFlow id="Flow_7573" name="" sourceRef="TaskNode_8386" targetRef="TaskNode_9243" /><bpmn:sequenceFlow id="Flow_3348" name="" sourceRef="TaskNode_9243" targetRef="GatewayNode_9150" /><bpmn:sequenceFlow id="Flow_3521" name="" sourceRef="GatewayNode_9150" targetRef="TaskNode_5724" /><bpmn:sequenceFlow id="Flow_4813" name="" sourceRef="GatewayNode_9150" targetRef="TaskNode_8201" /><bpmn:sequenceFlow id="Flow_7760" name="" sourceRef="GatewayNode_9150" targetRef="TaskNode_3143" /><bpmn:sequenceFlow id="Flow_2130" name="" sourceRef="TaskNode_3143" targetRef="GatewayNode_3367" /><bpmn:sequenceFlow id="Flow_3952" name="" sourceRef="TaskNode_8201" targetRef="GatewayNode_3367" /><bpmn:sequenceFlow id="Flow_7152" name="" sourceRef="TaskNode_5724" targetRef="GatewayNode_3367" /><bpmn:sequenceFlow id="Flow_9664" name="" sourceRef="GatewayNode_3367" targetRef="TaskNode_4815" /><bpmn:sequenceFlow id="Flow_6958" name="" sourceRef="TaskNode_4815" targetRef="EndNode_3956" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100"><bpmndi:BPMNShape id="BPMNShape_oqv809t_di" bpmnElement="StartNode_2236"><dc:Bounds height="36" width="36" x="240" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_shj6e6t_di" bpmnElement="TaskNode_8386"><dc:Bounds height="80" width="100" x="356" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_joy4d0a_di" bpmnElement="TaskNode_9243"><dc:Bounds height="80" width="100" x="536" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_q5zv9vc_di" bpmnElement="GatewayNode_9150"><dc:Bounds height="36" width="36" x="716" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_sxmgqtq_di" bpmnElement="TaskNode_5724"><dc:Bounds height="80" width="100" x="832" y="390" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_kqmkbpq_di" bpmnElement="TaskNode_8201"><dc:Bounds height="80" width="100" x="832" y="230" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_a6fz8cg_di" bpmnElement="TaskNode_3143"><dc:Bounds height="80" width="100" x="832" y="70" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_633bhpm_di" bpmnElement="GatewayNode_3367"><dc:Bounds height="36" width="36" x="1012" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_p9mlte7_di" bpmnElement="TaskNode_4815"><dc:Bounds height="80" width="100" x="1128" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_jkvvdci_di" bpmnElement="EndNode_3956"><dc:Bounds height="36" width="36" x="1308" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Flow_2449_di" bpmnElement="Flow_2449"><di:waypoint x="276" y="198" /><di:waypoint x="356" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_7573_di" bpmnElement="Flow_7573"><di:waypoint x="456" y="198" /><di:waypoint x="536" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_3348_di" bpmnElement="Flow_3348"><di:waypoint x="636" y="198" /><di:waypoint x="716" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_3521_di" bpmnElement="Flow_3521"><di:waypoint x="734" y="216" /><di:waypoint x="734" y="430" /><di:waypoint x="832" y="430" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_4813_di" bpmnElement="Flow_4813"><di:waypoint x="734" y="216" /><di:waypoint x="734" y="270" /><di:waypoint x="832" y="270" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_7760_di" bpmnElement="Flow_7760"><di:waypoint x="734" y="180" /><di:waypoint x="734" y="110" /><di:waypoint x="832" y="110" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_2130_di" bpmnElement="Flow_2130"><di:waypoint x="932" y="110" /><di:waypoint x="1030" y="110" /><di:waypoint x="1030" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_3952_di" bpmnElement="Flow_3952"><di:waypoint x="932" y="270" /><di:waypoint x="1030" y="270" /><di:waypoint x="1030" y="216" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_7152_di" bpmnElement="Flow_7152"><di:waypoint x="932" y="430" /><di:waypoint x="1030" y="430" /><di:waypoint x="1030" y="216" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_9664_di" bpmnElement="Flow_9664"><di:waypoint x="1048" y="198" /><di:waypoint x="1128" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_6958_di" bpmnElement="Flow_6958"><di:waypoint x="1228" y="198" /><di:waypoint x="1308" y="198" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, 0, NULL, 'fas fa-trash-alt', NULL, '2025-09-02 21:23:59+08', NULL, NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (15, 'Process_BookInvoiceReimbursement_6jgj', '1', '图书购书发票上传及报销流程', '图书购书发票上传及报销流程', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?> <bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL">   <bpmn:process id="Process_BookInvoiceReimbursement_6jgj" name="图书购书发票上传及报销流程" isExecutable="false">     <bpmn:startEvent id="AStartEvent" name="开始" />     <bpmn:userTask id="AActivity_UploadInvoice" name="上传发票" />     <bpmn:serviceTask id="AActivity_OCRExtract" name="OCR提取发票信息" />     <bpmn:exclusiveGateway id="AExclusiveGateway_CheckAmount" name="金额判断" />     <bpmn:userTask id="AActivity_ApproveByDepartmentManager" name="部门经理审批" />     <bpmn:userTask id="AActivity_ApproveByGeneralManager" name="总经理审批" />     <bpmn:userTask id="AActivity_FinanceApproval" name="财务经理签字报销" />     <bpmn:endEvent id="AEndEvent" name="结束" />     <bpmn:sequenceFlow id="Flow_AStartEvent_AActivity_UploadInvoice" sourceRef="AStartEvent" targetRef="AActivity_UploadInvoice" />     <bpmn:sequenceFlow id="Flow_AActivity_UploadInvoice_AActivity_OCRExtract" sourceRef="AActivity_UploadInvoice" targetRef="AActivity_OCRExtract" />     <bpmn:sequenceFlow id="Flow_AActivity_OCRExtract_AExclusiveGateway_CheckAmount" sourceRef="AActivity_OCRExtract" targetRef="AExclusiveGateway_CheckAmount" />     <bpmn:sequenceFlow id="Flow_AExclusiveGateway_CheckAmount_AActivity_ApproveByDepartmentManager" name="money_amount&#60;200" sourceRef="AExclusiveGateway_CheckAmount" targetRef="AActivity_ApproveByDepartmentManager">       <bpmn:conditionExpression>money_amount&lt;200</bpmn:conditionExpression>     </bpmn:sequenceFlow>     <bpmn:sequenceFlow id="Flow_AExclusiveGateway_CheckAmount_AActivity_ApproveByGeneralManager" name="money_amount&#62;=200" sourceRef="AExclusiveGateway_CheckAmount" targetRef="AActivity_ApproveByGeneralManager">       <bpmn:conditionExpression>money_amount&gt;=200</bpmn:conditionExpression>     </bpmn:sequenceFlow>     <bpmn:sequenceFlow id="Flow_AActivity_ApproveByDepartmentManager_AActivity_FinanceApproval" sourceRef="AActivity_ApproveByDepartmentManager" targetRef="AActivity_FinanceApproval" />     <bpmn:sequenceFlow id="Flow_AActivity_ApproveByGeneralManager_AActivity_FinanceApproval" sourceRef="AActivity_ApproveByGeneralManager" targetRef="AActivity_FinanceApproval" />     <bpmn:sequenceFlow id="Flow_AActivity_FinanceApproval_AEndEvent" sourceRef="AActivity_FinanceApproval" targetRef="AEndEvent" />   </bpmn:process>   <bpmndi:BPMNDiagram id="BPMNDiagram_1">     <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_BookInvoiceReimbursement_6jgj">       <bpmndi:BPMNShape id="Shape_AStartEvent" bpmnElement="AStartEvent">         <dc:Bounds x="100" y="100" width="36" height="36" />       </bpmndi:BPMNShape>       <bpmndi:BPMNShape id="Shape_AActivity_UploadInvoice" bpmnElement="AActivity_UploadInvoice">         <dc:Bounds x="300" y="100" width="100" height="80" />       </bpmndi:BPMNShape>       <bpmndi:BPMNShape id="Shape_AActivity_OCRExtract" bpmnElement="AActivity_OCRExtract">         <dc:Bounds x="500" y="100" width="100" height="80" />       </bpmndi:BPMNShape>       <bpmndi:BPMNShape id="Shape_AExclusiveGateway_CheckAmount" bpmnElement="AExclusiveGateway_CheckAmount" isMarkerVisible="true">         <dc:Bounds x="700" y="100" width="36" height="36" />       </bpmndi:BPMNShape>       <bpmndi:BPMNShape id="Shape_AActivity_ApproveByDepartmentManager" bpmnElement="AActivity_ApproveByDepartmentManager">         <dc:Bounds x="900" y="50" width="100" height="80" />       </bpmndi:BPMNShape>       <bpmndi:BPMNShape id="Shape_AActivity_ApproveByGeneralManager" bpmnElement="AActivity_ApproveByGeneralManager">         <dc:Bounds x="900" y="150" width="100" height="80" />       </bpmndi:BPMNShape>       <bpmndi:BPMNShape id="Shape_AActivity_FinanceApproval" bpmnElement="AActivity_FinanceApproval">         <dc:Bounds x="1100" y="100" width="100" height="80" />       </bpmndi:BPMNShape>       <bpmndi:BPMNShape id="Shape_AEndEvent" bpmnElement="AEndEvent">         <dc:Bounds x="1300" y="100" width="36" height="36" />       </bpmndi:BPMNShape>       <bpmndi:BPMNEdge id="Edge_AStartEvent_AActivity_UploadInvoice" bpmnElement="Flow_AStartEvent_AActivity_UploadInvoice">         <di:waypoint x="136" y="118" />         <di:waypoint x="218" y="118" />         <di:waypoint x="300" y="140" />       </bpmndi:BPMNEdge>       <bpmndi:BPMNEdge id="Edge_AActivity_UploadInvoice_AActivity_OCRExtract" bpmnElement="Flow_AActivity_UploadInvoice_AActivity_OCRExtract">         <di:waypoint x="400" y="140" />         <di:waypoint x="450" y="140" />         <di:waypoint x="500" y="140" />       </bpmndi:BPMNEdge>       <bpmndi:BPMNEdge id="Edge_AActivity_OCRExtract_AExclusiveGateway_CheckAmount" bpmnElement="Flow_AActivity_OCRExtract_AExclusiveGateway_CheckAmount">         <di:waypoint x="600" y="140" />         <di:waypoint x="650" y="140" />         <di:waypoint x="700" y="118" />       </bpmndi:BPMNEdge>       <bpmndi:BPMNEdge id="Edge_AExclusiveGateway_CheckAmount_AActivity_ApproveByDepartmentManager" bpmnElement="Flow_AExclusiveGateway_CheckAmount_AActivity_ApproveByDepartmentManager">         <di:waypoint x="736" y="118" />         <di:waypoint x="786" y="118" />         <di:waypoint x="786" y="90" />         <di:waypoint x="900" y="90" />         <bpmndi:BPMNLabel>           <dc:Bounds x="760" y="101" width="83" height="27" />         </bpmndi:BPMNLabel>       </bpmndi:BPMNEdge>       <bpmndi:BPMNEdge id="Edge_AExclusiveGateway_CheckAmount_AActivity_ApproveByGeneralManager" bpmnElement="Flow_AExclusiveGateway_CheckAmount_AActivity_ApproveByGeneralManager">         <di:waypoint x="736" y="118" />         <di:waypoint x="786" y="118" />         <di:waypoint x="786" y="190" />         <di:waypoint x="900" y="190" />         <bpmndi:BPMNLabel>           <dc:Bounds x="760" y="151" width="83" height="27" />         </bpmndi:BPMNLabel>       </bpmndi:BPMNEdge>       <bpmndi:BPMNEdge id="Edge_AActivity_ApproveByDepartmentManager_AActivity_FinanceApproval" bpmnElement="Flow_AActivity_ApproveByDepartmentManager_AActivity_FinanceApproval">         <di:waypoint x="1000" y="90" />         <di:waypoint x="1050" y="90" />         <di:waypoint x="1050" y="140" />         <di:waypoint x="1100" y="140" />       </bpmndi:BPMNEdge>       <bpmndi:BPMNEdge id="Edge_AActivity_ApproveByGeneralManager_AActivity_FinanceApproval" bpmnElement="Flow_AActivity_ApproveByGeneralManager_AActivity_FinanceApproval">         <di:waypoint x="1000" y="190" />         <di:waypoint x="1050" y="190" />         <di:waypoint x="1050" y="140" />         <di:waypoint x="1100" y="140" />       </bpmndi:BPMNEdge>       <bpmndi:BPMNEdge id="Edge_AActivity_FinanceApproval_AEndEvent" bpmnElement="Flow_AActivity_FinanceApproval_AEndEvent">         <di:waypoint x="1200" y="140" />         <di:waypoint x="1250" y="140" />         <di:waypoint x="1300" y="118" />       </bpmndi:BPMNEdge>     </bpmndi:BPMNPlane>   </bpmndi:BPMNDiagram> </bpmn:definitions> ', 0, NULL, 0, NULL, 'fas fa-file-invoice', NULL, '2025-09-02 23:00:10+08', NULL, NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (17, 'EOrderProcess_4105', '1', 'EOrderProcess', 'Process_Code_4105', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?> <bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">   <bpmn:process id="EOrderProcess_4105" sf:code="Process_Code_4105" name="EOrderProcess" isExecutable="true" sf:version="1">     <bpmn:startEvent id="StartEvent_1" name="Start">       <bpmn:outgoing>Flow_0gc2nrp</bpmn:outgoing>     </bpmn:startEvent>     <bpmn:task id="Activity_1d9fhwm" sf:code="Dispatching" name="Dispatch">       <bpmn:extensionElements>         <sf:performers>           <sf:performer name="业务员" outerId="9" outerCode="salesmate" outerType="Role" />         </sf:performers>       </bpmn:extensionElements>       <bpmn:incoming>Flow_0gc2nrp</bpmn:incoming>       <bpmn:outgoing>Flow_11dnx0o</bpmn:outgoing>     </bpmn:task>     <bpmn:sequenceFlow id="Flow_0gc2nrp" sourceRef="StartEvent_1" targetRef="Activity_1d9fhwm" />     <bpmn:sequenceFlow id="Flow_11dnx0o" sourceRef="Activity_1d9fhwm" targetRef="Gateway_00mxluj" />     <bpmn:task id="Activity_1a9gp55" sf:code="Delivering" name="Print Delivery Note">       <bpmn:extensionElements>         <sf:performers>           <sf:performer name="包装员" outerId="13" outerCode="expressmate" outerType="Role" />         </sf:performers>       </bpmn:extensionElements>       <bpmn:incoming>Flow_0nmgmle</bpmn:incoming>       <bpmn:outgoing>Flow_06bgmzl</bpmn:outgoing>     </bpmn:task>     <bpmn:sequenceFlow id="Flow_0nmgmle" sourceRef="Gateway_00mxluj" targetRef="Activity_1a9gp55">       <bpmn:conditionExpression>CanUseStock == "true" &amp;&amp; IsHavingWeight == "true"</bpmn:conditionExpression>     </bpmn:sequenceFlow>     <bpmn:task id="Activity_0hkgchv" sf:code="Sampling" name="Sample">       <bpmn:extensionElements>         <sf:performers>           <sf:performer name="打样员" outerId="10" outerCode="techmate" outerType="Role" />         </sf:performers>       </bpmn:extensionElements>       <bpmn:incoming>Flow_09uxdoe</bpmn:incoming>       <bpmn:outgoing>Flow_0qzd43m</bpmn:outgoing>     </bpmn:task>     <bpmn:sequenceFlow id="Flow_09uxdoe" sourceRef="Gateway_00mxluj" targetRef="Activity_0hkgchv">       <bpmn:conditionExpression>CanUseStock == "false" &amp;&amp; IsHavingWeight == "false"</bpmn:conditionExpression>     </bpmn:sequenceFlow>     <bpmn:endEvent id="Event_19jb4sc" name="End">       <bpmn:incoming>Flow_06bgmzl</bpmn:incoming>       <bpmn:incoming>Flow_1274c57</bpmn:incoming>     </bpmn:endEvent>     <bpmn:sequenceFlow id="Flow_06bgmzl" sourceRef="Activity_1a9gp55" targetRef="Event_19jb4sc" />     <bpmn:task id="Activity_1bm1f4i" sf:code="Manufacturing" name="Manufacture">       <bpmn:extensionElements>         <sf:performers>           <sf:performer name="跟单员" outerId="11" outerCode="merchandiser" outerType="Role" />         </sf:performers>       </bpmn:extensionElements>       <bpmn:incoming>Flow_0qzd43m</bpmn:incoming>       <bpmn:outgoing>Flow_11j812b</bpmn:outgoing>     </bpmn:task>     <bpmn:sequenceFlow id="Flow_0qzd43m" sourceRef="Activity_0hkgchv" targetRef="Activity_1bm1f4i" />     <bpmn:task id="Activity_009vdps" sf:code="QCChecking" name="QA">       <bpmn:extensionElements>         <sf:performers>           <sf:performer name="质检员" outerId="12" outerCode="qcmate" outerType="Role" />         </sf:performers>       </bpmn:extensionElements>       <bpmn:incoming>Flow_11j812b</bpmn:incoming>       <bpmn:outgoing>Flow_1c5t6cw</bpmn:outgoing>     </bpmn:task>     <bpmn:sequenceFlow id="Flow_11j812b" sourceRef="Activity_1bm1f4i" targetRef="Activity_009vdps" />     <bpmn:task id="Activity_1oihz5s" sf:code="Weighting" name="Weight">       <bpmn:extensionElements>         <sf:performers>           <sf:performer name="包装员" outerId="13" outerCode="expressmate" outerType="Role" />         </sf:performers>       </bpmn:extensionElements>       <bpmn:incoming>Flow_1c5t6cw</bpmn:incoming>       <bpmn:outgoing>Flow_1274c57</bpmn:outgoing>     </bpmn:task>     <bpmn:sequenceFlow id="Flow_1c5t6cw" sourceRef="Activity_009vdps" targetRef="Activity_1oihz5s" />     <bpmn:sequenceFlow id="Flow_1274c57" sourceRef="Activity_1oihz5s" targetRef="Event_19jb4sc" />     <bpmn:inclusiveGateway id="Gateway_00mxluj">       <bpmn:incoming>Flow_11dnx0o</bpmn:incoming>       <bpmn:outgoing>Flow_0nmgmle</bpmn:outgoing>       <bpmn:outgoing>Flow_09uxdoe</bpmn:outgoing>     </bpmn:inclusiveGateway>   </bpmn:process>   <bpmndi:BPMNDiagram id="BPMNDiagram_1">     <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="EOrderProcess_4105">       <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">         <dc:Bounds x="202" y="240" width="36" height="36" />         <bpmndi:BPMNLabel>           <dc:Bounds x="208" y="276" width="24" height="14" />         </bpmndi:BPMNLabel>       </bpmndi:BPMNShape>       <bpmndi:BPMNShape id="Activity_1d9fhwm_di" bpmnElement="Activity_1d9fhwm">         <dc:Bounds x="290" y="218" width="100" height="80" />         <bpmndi:BPMNLabel />       </bpmndi:BPMNShape>       <bpmndi:BPMNShape id="Activity_1a9gp55_di" bpmnElement="Activity_1a9gp55">         <dc:Bounds x="760" y="60" width="100" height="80" />         <bpmndi:BPMNLabel />       </bpmndi:BPMNShape>       <bpmndi:BPMNShape id="Activity_0hkgchv_di" bpmnElement="Activity_0hkgchv">         <dc:Bounds x="580" y="218" width="100" height="80" />         <bpmndi:BPMNLabel />       </bpmndi:BPMNShape>       <bpmndi:BPMNShape id="Event_19jb4sc_di" bpmnElement="Event_19jb4sc">         <dc:Bounds x="1352" y="240" width="36" height="36" />         <bpmndi:BPMNLabel>           <dc:Bounds x="1360" y="283" width="20" height="14" />         </bpmndi:BPMNLabel>       </bpmndi:BPMNShape>       <bpmndi:BPMNShape id="Activity_1bm1f4i_di" bpmnElement="Activity_1bm1f4i">         <dc:Bounds x="790" y="218" width="100" height="80" />         <bpmndi:BPMNLabel />       </bpmndi:BPMNShape>       <bpmndi:BPMNShape id="Activity_009vdps_di" bpmnElement="Activity_009vdps">         <dc:Bounds x="956" y="218" width="100" height="80" />         <bpmndi:BPMNLabel />       </bpmndi:BPMNShape>       <bpmndi:BPMNShape id="Activity_1oihz5s_di" bpmnElement="Activity_1oihz5s">         <dc:Bounds x="1130" y="218" width="100" height="80" />         <bpmndi:BPMNLabel />       </bpmndi:BPMNShape>       <bpmndi:BPMNShape id="Gateway_13pu7tp_di" bpmnElement="Gateway_00mxluj">         <dc:Bounds x="475" y="233" width="50" height="50" />       </bpmndi:BPMNShape>       <bpmndi:BPMNEdge id="Flow_0gc2nrp_di" bpmnElement="Flow_0gc2nrp">         <di:waypoint x="238" y="258" />         <di:waypoint x="290" y="258" />       </bpmndi:BPMNEdge>       <bpmndi:BPMNEdge id="Flow_11dnx0o_di" bpmnElement="Flow_11dnx0o">         <di:waypoint x="390" y="258" />         <di:waypoint x="475" y="258" />       </bpmndi:BPMNEdge>       <bpmndi:BPMNEdge id="Flow_0nmgmle_di" bpmnElement="Flow_0nmgmle">         <di:waypoint x="500" y="233" />         <di:waypoint x="500" y="100" />         <di:waypoint x="760" y="100" />       </bpmndi:BPMNEdge>       <bpmndi:BPMNEdge id="Flow_09uxdoe_di" bpmnElement="Flow_09uxdoe">         <di:waypoint x="525" y="258" />         <di:waypoint x="580" y="258" />       </bpmndi:BPMNEdge>       <bpmndi:BPMNEdge id="Flow_06bgmzl_di" bpmnElement="Flow_06bgmzl">         <di:waypoint x="860" y="100" />         <di:waypoint x="1370" y="100" />         <di:waypoint x="1370" y="240" />       </bpmndi:BPMNEdge>       <bpmndi:BPMNEdge id="Flow_0qzd43m_di" bpmnElement="Flow_0qzd43m">         <di:waypoint x="680" y="258" />         <di:waypoint x="790" y="258" />       </bpmndi:BPMNEdge>       <bpmndi:BPMNEdge id="Flow_11j812b_di" bpmnElement="Flow_11j812b">         <di:waypoint x="890" y="258" />         <di:waypoint x="956" y="258" />       </bpmndi:BPMNEdge>       <bpmndi:BPMNEdge id="Flow_1c5t6cw_di" bpmnElement="Flow_1c5t6cw">         <di:waypoint x="1056" y="258" />         <di:waypoint x="1130" y="258" />       </bpmndi:BPMNEdge>       <bpmndi:BPMNEdge id="Flow_1274c57_di" bpmnElement="Flow_1274c57">         <di:waypoint x="1230" y="258" />         <di:waypoint x="1352" y="258" />       </bpmndi:BPMNEdge>     </bpmndi:BPMNPlane>   </bpmndi:BPMNDiagram> </bpmn:definitions> ', 0, NULL, 0, NULL, 'fas fa-envelope', NULL, '2025-09-03 08:24:28+08', NULL, NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (18, 'Process_SummerStudyTour_8tw0', '1', '暑假游学活动流程', 'Process_SummerStudyTour_8tw0', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="Process_SummerStudyTour_8tw0" name="暑假游学活动流程" isExecutable="false"><bpmn:startEvent id="AStartEvent" name="开始" /><bpmn:userTask id="AActivity_Registration" name="报名及信息登记" /><bpmn:userTask id="AActivity_Payment" name="缴纳费用" /><bpmn:userTask id="AActivity_Confirmation" name="报名确认" /><bpmn:userTask id="AActivity_Preparation" name="行前准备" /><bpmn:userTask id="AActivity_Tour" name="游学活动进行中" /><bpmn:userTask id="AActivity_Feedback" name="提交反馈" /><bpmn:endEvent id="AEndEvent" name="结束" /><bpmn:sequenceFlow id="Flow_AStartEvent_AActivity_Registration" sourceRef="AStartEvent" targetRef="AActivity_Registration" /><bpmn:sequenceFlow id="Flow_AActivity_Registration_AActivity_Payment" sourceRef="AActivity_Registration" targetRef="AActivity_Payment" /><bpmn:sequenceFlow id="Flow_AActivity_Payment_AActivity_Confirmation" sourceRef="AActivity_Payment" targetRef="AActivity_Confirmation" /><bpmn:sequenceFlow id="Flow_AActivity_Confirmation_AActivity_Preparation" sourceRef="AActivity_Confirmation" targetRef="AActivity_Preparation" /><bpmn:sequenceFlow id="Flow_AActivity_Preparation_AActivity_Tour" sourceRef="AActivity_Preparation" targetRef="AActivity_Tour" /><bpmn:sequenceFlow id="Flow_AActivity_Tour_AActivity_Feedback" sourceRef="AActivity_Tour" targetRef="AActivity_Feedback" /><bpmn:sequenceFlow id="Flow_AActivity_Feedback_AEndEvent" sourceRef="AActivity_Feedback" targetRef="AEndEvent" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_SummerStudyTour_8tw0"><bpmndi:BPMNShape id="Shape_AStartEvent" bpmnElement="AStartEvent"><dc:Bounds x="100" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_Registration" bpmnElement="AActivity_Registration"><dc:Bounds x="300" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_Payment" bpmnElement="AActivity_Payment"><dc:Bounds x="500" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_Confirmation" bpmnElement="AActivity_Confirmation"><dc:Bounds x="700" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_Preparation" bpmnElement="AActivity_Preparation"><dc:Bounds x="900" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_Tour" bpmnElement="AActivity_Tour"><dc:Bounds x="1100" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_Feedback" bpmnElement="AActivity_Feedback"><dc:Bounds x="1300" y="100" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent" bpmnElement="AEndEvent"><dc:Bounds x="1500" y="100" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AStartEvent_AActivity_Registration" bpmnElement="Flow_AStartEvent_AActivity_Registration"><di:waypoint x="136" y="118" /><di:waypoint x="218" y="118" /><di:waypoint x="300" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_Registration_AActivity_Payment" bpmnElement="Flow_AActivity_Registration_AActivity_Payment"><di:waypoint x="400" y="140" /><di:waypoint x="450" y="140" /><di:waypoint x="500" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_Payment_AActivity_Confirmation" bpmnElement="Flow_AActivity_Payment_AActivity_Confirmation"><di:waypoint x="600" y="140" /><di:waypoint x="650" y="140" /><di:waypoint x="700" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_Confirmation_AActivity_Preparation" bpmnElement="Flow_AActivity_Confirmation_AActivity_Preparation"><di:waypoint x="800" y="140" /><di:waypoint x="850" y="140" /><di:waypoint x="900" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_Preparation_AActivity_Tour" bpmnElement="Flow_AActivity_Preparation_AActivity_Tour"><di:waypoint x="1000" y="140" /><di:waypoint x="1050" y="140" /><di:waypoint x="1100" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_Tour_AActivity_Feedback" bpmnElement="Flow_AActivity_Tour_AActivity_Feedback"><di:waypoint x="1200" y="140" /><di:waypoint x="1250" y="140" /><di:waypoint x="1300" y="140" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_Feedback_AEndEvent" bpmnElement="Flow_AActivity_Feedback_AEndEvent"><di:waypoint x="1400" y="140" /><di:waypoint x="1450" y="140" /><di:waypoint x="1500" y="118" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, 0, NULL, NULL, NULL, '2025-09-08 16:07:00+08', NULL, NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (19, 'Process_3350', '1', 'Process_Name_3350', 'Process_Code_3350', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_3350" sf:code="Process_Code_3350" name="Process_Name_3350" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="StartEvent_1" name="Start(申请者)">
      <bpmn:extensionElements>
        <sf:sections>
          <sf:section name="myProperties">adi_case_flag==''Y''</sf:section>
        </sf:sections>
      </bpmn:extensionElements>
      <bpmn:outgoing>Flow_1udmara</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:task id="Activity_1bxsgwg" name="黄光工程师">
      <bpmn:incoming>Flow_0oqekn6</bpmn:incoming>
      <bpmn:incoming>Flow_1tp1rws</bpmn:incoming>
      <bpmn:outgoing>Flow_0dyyinx</bpmn:outgoing>
    </bpmn:task>
    <bpmn:task id="Activity_00yyue8" name="黄光复核存储">
      <bpmn:incoming>Flow_0dyyinx</bpmn:incoming>
      <bpmn:outgoing>Flow_0dicvil</bpmn:outgoing>
    </bpmn:task>
    <bpmn:task id="Activity_0gdpp9o" name="申请者">
      <bpmn:incoming>Flow_0dicvil</bpmn:incoming>
      <bpmn:outgoing>Flow_059a31g</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_0dicvil" sourceRef="Activity_00yyue8" targetRef="Activity_0gdpp9o" />
    <bpmn:exclusiveGateway id="Gateway_1tmkd46">
      <bpmn:incoming>Flow_059a31g</bpmn:incoming>
      <bpmn:outgoing>Flow_1jsdadk</bpmn:outgoing>
      <bpmn:outgoing>Flow_1xstu9n</bpmn:outgoing>
    </bpmn:exclusiveGateway>
    <bpmn:sequenceFlow id="Flow_059a31g" sourceRef="Activity_0gdpp9o" targetRef="Gateway_1tmkd46" />
    <bpmn:sequenceFlow id="Flow_1jsdadk" name="is_adi_done==&#39;Y&#39;" sourceRef="Gateway_1tmkd46" targetRef="Event_0wl3if9">
      <bpmn:conditionExpression>is_adi_done=="Y"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:endEvent id="Event_0wl3if9" name="End">
      <bpmn:incoming>Flow_1jsdadk</bpmn:incoming>
      <bpmn:incoming>Flow_1abqg2h</bpmn:incoming>
      <bpmn:incoming>Flow_1tfd8ic</bpmn:incoming>
      <bpmn:incoming>Flow_0gazemr</bpmn:incoming>
      <bpmn:incoming>Flow_1es7c32</bpmn:incoming>
      <bpmn:incoming>Flow_1mx5loj</bpmn:incoming>
      <bpmn:incoming>Flow_0ws7ij4</bpmn:incoming>
      <bpmn:incoming>Flow_13h3eu2</bpmn:incoming>
      <bpmn:incoming>Flow_0ktpapa</bpmn:incoming>
      <bpmn:incoming>Flow_0x9fkpe</bpmn:incoming>
      <bpmn:incoming>Flow_00okasn</bpmn:incoming>
      <bpmn:incoming>Flow_0rr7zbp</bpmn:incoming>
      <bpmn:incoming>Flow_0heysg2</bpmn:incoming>
      <bpmn:incoming>Flow_0hcihmf</bpmn:incoming>
      <bpmn:incoming>Flow_1fljcjd</bpmn:incoming>
      <bpmn:incoming>Flow_1gnfpjw</bpmn:incoming>
      <bpmn:incoming>Flow_1ov5k40</bpmn:incoming>
      <bpmn:incoming>Flow_1w9tz8x</bpmn:incoming>
    </bpmn:endEvent>
    <bpmn:sequenceFlow id="Flow_1xstu9n" name="is_adi_done==&#39;N&#39;" sourceRef="Gateway_1tmkd46" targetRef="Gateway_0psd4cv">
      <bpmn:conditionExpression>is_adi_done=="N"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:exclusiveGateway id="Gateway_0psd4cv">
      <bpmn:incoming>Flow_1xstu9n</bpmn:incoming>
      <bpmn:outgoing>Flow_0gbmu89</bpmn:outgoing>
      <bpmn:outgoing>Flow_19ty7g4</bpmn:outgoing>
      <bpmn:outgoing>Flow_1dz4ops</bpmn:outgoing>
      <bpmn:outgoing>Flow_1ha7z3f</bpmn:outgoing>
    </bpmn:exclusiveGateway>
    <bpmn:task id="Activity_0w3e41w" name="申请者(AEI)">
      <bpmn:incoming>Flow_0gbmu89</bpmn:incoming>
      <bpmn:outgoing>Flow_1elwptq</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_0gbmu89" name="adi_aei==&#39;Y&#39;" sourceRef="Gateway_0psd4cv" targetRef="Activity_0w3e41w">
      <bpmn:conditionExpression>adi_aei=="Y"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:task id="Activity_01paanv" name="申请者(WAT)">
      <bpmn:incoming>Flow_19ty7g4</bpmn:incoming>
      <bpmn:outgoing>Flow_1h7yjfz</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_19ty7g4" name="adi_wat==&#39;Y&#39;" sourceRef="Gateway_0psd4cv" targetRef="Activity_01paanv">
      <bpmn:conditionExpression>adi_wat=="Y"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:task id="Activity_08j2a4w" name="申请者(CP)">
      <bpmn:incoming>Flow_1dz4ops</bpmn:incoming>
      <bpmn:outgoing>Flow_0ws7ij4</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_1dz4ops" name="adi_cp==&#39;Y&#39;" sourceRef="Gateway_0psd4cv" targetRef="Activity_08j2a4w">
      <bpmn:conditionExpression>adi_cp=="Y"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:exclusiveGateway id="Gateway_0b55ajv">
      <bpmn:incoming>Flow_1elwptq</bpmn:incoming>
      <bpmn:outgoing>Flow_0jgw3lc</bpmn:outgoing>
      <bpmn:outgoing>Flow_1abqg2h</bpmn:outgoing>
    </bpmn:exclusiveGateway>
    <bpmn:sequenceFlow id="Flow_1elwptq" name="" sourceRef="Activity_0w3e41w" targetRef="Gateway_0b55ajv" />
    <bpmn:task id="Activity_08luf37" name="申请者(KLA、WAT、CP)">
      <bpmn:incoming>Flow_0jgw3lc</bpmn:incoming>
      <bpmn:outgoing>Flow_003pgvq</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_0jgw3lc" sourceRef="Gateway_0b55ajv" targetRef="Activity_08luf37" />
    <bpmn:sequenceFlow id="Flow_1abqg2h" name="is_adi_aei_done==&#39;Y&#39;" sourceRef="Gateway_0b55ajv" targetRef="Event_0wl3if9">
      <bpmn:conditionExpression>is_adi_aei_done==''Y''</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:exclusiveGateway id="Gateway_0h33o64">
      <bpmn:incoming>Flow_003pgvq</bpmn:incoming>
      <bpmn:outgoing>Flow_1az2998</bpmn:outgoing>
      <bpmn:outgoing>Flow_1fpxn4w</bpmn:outgoing>
      <bpmn:outgoing>Flow_0pa2mca</bpmn:outgoing>
      <bpmn:outgoing>Flow_1iio5g2</bpmn:outgoing>
    </bpmn:exclusiveGateway>
    <bpmn:sequenceFlow id="Flow_003pgvq" sourceRef="Activity_08luf37" targetRef="Gateway_0h33o64" />
    <bpmn:task id="Activity_07w85xm" name="YED工程师">
      <bpmn:incoming>Flow_15x2z9b</bpmn:incoming>
      <bpmn:incoming>Flow_1iio5g2</bpmn:incoming>
      <bpmn:outgoing>Flow_0g8rhrk</bpmn:outgoing>
    </bpmn:task>
    <bpmn:task id="Activity_0r32pum" name="申请者(WAT)">
      <bpmn:incoming>Flow_1az2998</bpmn:incoming>
      <bpmn:outgoing>Flow_1pkxdjz</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_1az2998" name="adi_aei_wat==&#39;Y&#39;" sourceRef="Gateway_0h33o64" targetRef="Activity_0r32pum">
      <bpmn:conditionExpression>adi_aei_wat=="Y"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:task id="Activity_0dku4wx" name="申请者(CP)">
      <bpmn:incoming>Flow_1fpxn4w</bpmn:incoming>
      <bpmn:outgoing>Flow_0gazemr</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_1fpxn4w" name="adi_aei_cp==&#39;Y&#39;" sourceRef="Gateway_0h33o64" targetRef="Activity_0dku4wx">
      <bpmn:conditionExpression>adi_aei_cp=="Y"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:task id="Activity_0f7u3bd" name="YED复核存储">
      <bpmn:incoming>Flow_0g8rhrk</bpmn:incoming>
      <bpmn:outgoing>Flow_0n9sd9r</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_0g8rhrk" sourceRef="Activity_07w85xm" targetRef="Activity_0f7u3bd" />
    <bpmn:task id="Activity_1y36h74" name="申请者(KLA)">
      <bpmn:incoming>Flow_0n9sd9r</bpmn:incoming>
      <bpmn:outgoing>Flow_127f5oi</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_0n9sd9r" sourceRef="Activity_0f7u3bd" targetRef="Activity_1y36h74" />
    <bpmn:exclusiveGateway id="Gateway_1urue73">
      <bpmn:incoming>Flow_1sn6vco</bpmn:incoming>
      <bpmn:outgoing>Flow_1tfd8ic</bpmn:outgoing>
      <bpmn:outgoing>Flow_0345qun</bpmn:outgoing>
    </bpmn:exclusiveGateway>
    <bpmn:sequenceFlow id="Flow_1tfd8ic" name="is_adi_aei_kla_done==&#39;Y&#39;" sourceRef="Gateway_1urue73" targetRef="Event_0wl3if9">
      <bpmn:conditionExpression>is_adi_aei_kla_done=="Y"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:task id="Activity_1bivb6l" name="申请者(CP)">
      <bpmn:incoming>Flow_0345qun</bpmn:incoming>
      <bpmn:outgoing>Flow_1w9tz8x</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_0345qun" name="is_adi_aei_kla_wat_cp==&#39;Y&#39;" sourceRef="Gateway_1urue73" targetRef="Activity_1bivb6l">
      <bpmn:conditionExpression>is_adi_aei_kla_wat_cp=="Y"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:sequenceFlow id="Flow_0gazemr" sourceRef="Activity_0dku4wx" targetRef="Event_0wl3if9" />
    <bpmn:exclusiveGateway id="Gateway_0n0qla0">
      <bpmn:incoming>Flow_1pkxdjz</bpmn:incoming>
      <bpmn:outgoing>Flow_01pulbz</bpmn:outgoing>
      <bpmn:outgoing>Flow_1es7c32</bpmn:outgoing>
    </bpmn:exclusiveGateway>
    <bpmn:sequenceFlow id="Flow_1pkxdjz" sourceRef="Activity_0r32pum" targetRef="Gateway_0n0qla0" />
    <bpmn:task id="Activity_1c8y7s3" name="申请者(CP)">
      <bpmn:incoming>Flow_01pulbz</bpmn:incoming>
      <bpmn:outgoing>Flow_1mx5loj</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_01pulbz" sourceRef="Gateway_0n0qla0" targetRef="Activity_1c8y7s3" />
    <bpmn:sequenceFlow id="Flow_1es7c32" name="is_adi_aei_wat_done==&#39;Y&#39;" sourceRef="Gateway_0n0qla0" targetRef="Event_0wl3if9">
      <bpmn:conditionExpression>is_adi_aei_wat_done=="Y"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:sequenceFlow id="Flow_1mx5loj" sourceRef="Activity_1c8y7s3" targetRef="Event_0wl3if9" />
    <bpmn:sequenceFlow id="Flow_0ws7ij4" sourceRef="Activity_08j2a4w" targetRef="Event_0wl3if9" />
    <bpmn:task id="Activity_017w2ns" name="申请者(AEI、WAT、CP)">
      <bpmn:incoming>Flow_0y41dvf</bpmn:incoming>
      <bpmn:outgoing>Flow_06f13zd</bpmn:outgoing>
    </bpmn:task>
    <bpmn:exclusiveGateway id="Gateway_0ibh3r4">
      <bpmn:incoming>Flow_06f13zd</bpmn:incoming>
      <bpmn:outgoing>Flow_0u9hvox</bpmn:outgoing>
      <bpmn:outgoing>Flow_19zvk4a</bpmn:outgoing>
      <bpmn:outgoing>Flow_1s2e7oq</bpmn:outgoing>
      <bpmn:outgoing>Flow_1fljcjd</bpmn:outgoing>
    </bpmn:exclusiveGateway>
    <bpmn:sequenceFlow id="Flow_06f13zd" sourceRef="Activity_017w2ns" targetRef="Gateway_0ibh3r4" />
    <bpmn:task id="Activity_0tw5log" name="申请者(AEI)">
      <bpmn:incoming>Flow_0u9hvox</bpmn:incoming>
      <bpmn:outgoing>Flow_0zr7cde</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_0u9hvox" name="adi_kla_aei==&#39;Y&#39;" sourceRef="Gateway_0ibh3r4" targetRef="Activity_0tw5log">
      <bpmn:conditionExpression>adi_kla_aei=="Y"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:task id="Activity_0dk6cem" name="申请者(WAT)">
      <bpmn:incoming>Flow_19zvk4a</bpmn:incoming>
      <bpmn:outgoing>Flow_1j2620d</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_19zvk4a" name="adi_kla_wat==&#39;Y&#39;" sourceRef="Gateway_0ibh3r4" targetRef="Activity_0dk6cem">
      <bpmn:conditionExpression>adi_kla_wat=="Y"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:task id="Activity_1l1pkbd" name="申请者(CP)">
      <bpmn:incoming>Flow_1s2e7oq</bpmn:incoming>
      <bpmn:outgoing>Flow_00okasn</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_1s2e7oq" name="adi_kla_cp==&#39;Y&#39;" sourceRef="Gateway_0ibh3r4" targetRef="Activity_1l1pkbd">
      <bpmn:conditionExpression>adi_kla_cp=="Y"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:sequenceFlow id="Flow_0dyyinx" sourceRef="Activity_1bxsgwg" targetRef="Activity_00yyue8" />
    <bpmn:task id="Activity_0ljvldo" name="YED工程师">
      <bpmn:incoming>Flow_05xracx</bpmn:incoming>
      <bpmn:incoming>Flow_16gw92s</bpmn:incoming>
      <bpmn:outgoing>Flow_0w9uvsj</bpmn:outgoing>
    </bpmn:task>
    <bpmn:task id="Activity_0uyucp2" name="YED复核存储">
      <bpmn:incoming>Flow_0w9uvsj</bpmn:incoming>
      <bpmn:outgoing>Flow_0y41dvf</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_0w9uvsj" sourceRef="Activity_0ljvldo" targetRef="Activity_0uyucp2" />
    <bpmn:sequenceFlow id="Flow_0y41dvf" sourceRef="Activity_0uyucp2" targetRef="Activity_017w2ns" />
    <bpmn:exclusiveGateway id="Gateway_04md141">
      <bpmn:incoming>Flow_0zr7cde</bpmn:incoming>
      <bpmn:outgoing>Flow_1ivg1nv</bpmn:outgoing>
      <bpmn:outgoing>Flow_13h3eu2</bpmn:outgoing>
    </bpmn:exclusiveGateway>
    <bpmn:sequenceFlow id="Flow_0zr7cde" sourceRef="Activity_0tw5log" targetRef="Gateway_04md141" />
    <bpmn:task id="Activity_16sqpei" name="申请者(WAT)">
      <bpmn:incoming>Flow_1ivg1nv</bpmn:incoming>
      <bpmn:outgoing>Flow_0lp6ozk</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_1ivg1nv" sourceRef="Gateway_04md141" targetRef="Activity_16sqpei" />
    <bpmn:sequenceFlow id="Flow_13h3eu2" name="is_adi_kla_aei_done==&#39;Y&#39;" sourceRef="Gateway_04md141" targetRef="Event_0wl3if9">
      <bpmn:conditionExpression>is_adi_kla_aei_done=="Y"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:exclusiveGateway id="Gateway_1vzgw1q">
      <bpmn:incoming>Flow_0lp6ozk</bpmn:incoming>
      <bpmn:outgoing>Flow_06efhc0</bpmn:outgoing>
      <bpmn:outgoing>Flow_0ktpapa</bpmn:outgoing>
    </bpmn:exclusiveGateway>
    <bpmn:sequenceFlow id="Flow_0lp6ozk" sourceRef="Activity_16sqpei" targetRef="Gateway_1vzgw1q" />
    <bpmn:task id="Activity_1mrdlz0" name="申请者(CP)">
      <bpmn:incoming>Flow_06efhc0</bpmn:incoming>
      <bpmn:outgoing>Flow_0x9fkpe</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_06efhc0" sourceRef="Gateway_1vzgw1q" targetRef="Activity_1mrdlz0" />
    <bpmn:sequenceFlow id="Flow_0ktpapa" name="is_adi_kla_aei_wat_done==&#39;Y&#39;" sourceRef="Gateway_1vzgw1q" targetRef="Event_0wl3if9">
      <bpmn:conditionExpression>is_adi_kla_aei_wat_done=="Y"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:sequenceFlow id="Flow_0x9fkpe" sourceRef="Activity_1mrdlz0" targetRef="Event_0wl3if9" />
    <bpmn:sequenceFlow id="Flow_00okasn" sourceRef="Activity_1l1pkbd" targetRef="Event_0wl3if9" />
    <bpmn:exclusiveGateway id="Gateway_1f38fy0">
      <bpmn:incoming>Flow_1j2620d</bpmn:incoming>
      <bpmn:outgoing>Flow_1wpqqls</bpmn:outgoing>
      <bpmn:outgoing>Flow_1gnfpjw</bpmn:outgoing>
    </bpmn:exclusiveGateway>
    <bpmn:sequenceFlow id="Flow_1j2620d" sourceRef="Activity_0dk6cem" targetRef="Gateway_1f38fy0" />
    <bpmn:task id="Activity_12qzdzn" name="申请者(CP)">
      <bpmn:incoming>Flow_1wpqqls</bpmn:incoming>
      <bpmn:outgoing>Flow_0rr7zbp</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_1wpqqls" sourceRef="Gateway_1f38fy0" targetRef="Activity_12qzdzn" />
    <bpmn:sequenceFlow id="Flow_0rr7zbp" sourceRef="Activity_12qzdzn" targetRef="Event_0wl3if9" />
    <bpmn:exclusiveGateway id="Gateway_1rfs9b7">
      <bpmn:incoming>Flow_1h7yjfz</bpmn:incoming>
      <bpmn:outgoing>Flow_1swtbm9</bpmn:outgoing>
      <bpmn:outgoing>Flow_0heysg2</bpmn:outgoing>
    </bpmn:exclusiveGateway>
    <bpmn:sequenceFlow id="Flow_1h7yjfz" sourceRef="Activity_01paanv" targetRef="Gateway_1rfs9b7" />
    <bpmn:task id="Activity_1qcdlr8" name="申请者(CP)">
      <bpmn:incoming>Flow_1swtbm9</bpmn:incoming>
      <bpmn:outgoing>Flow_0hcihmf</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_1swtbm9" sourceRef="Gateway_1rfs9b7" targetRef="Activity_1qcdlr8" />
    <bpmn:sequenceFlow id="Flow_0heysg2" name="is_adi_wat_done==&#39;Y&#39;" sourceRef="Gateway_1rfs9b7" targetRef="Event_0wl3if9">
      <bpmn:conditionExpression>is_adi_wat_done=="Y"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:sequenceFlow id="Flow_0hcihmf" sourceRef="Activity_1qcdlr8" targetRef="Event_0wl3if9" />
    <bpmn:sequenceFlow id="Flow_1fljcjd" name="is_adi_kla_done==&#39;Y&#39;" sourceRef="Gateway_0ibh3r4" targetRef="Event_0wl3if9">
      <bpmn:conditionExpression>is_adi_kla_done=="Y"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:sequenceFlow id="Flow_1gnfpjw" name="is_adi_kla_wat_done==&#39;Y&#39;" sourceRef="Gateway_1f38fy0" targetRef="Event_0wl3if9">
      <bpmn:conditionExpression>is_adi_kla_wat_done=="Y"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:exclusiveGateway id="Gateway_00ttzeq">
      <bpmn:incoming>Flow_127f5oi</bpmn:incoming>
      <bpmn:outgoing>Flow_0ge78ea</bpmn:outgoing>
      <bpmn:outgoing>Flow_0q96hlv</bpmn:outgoing>
    </bpmn:exclusiveGateway>
    <bpmn:sequenceFlow id="Flow_127f5oi" sourceRef="Activity_1y36h74" targetRef="Gateway_00ttzeq" />
    <bpmn:task id="Activity_14mukh3" name="申请者(WAT)">
      <bpmn:incoming>Flow_0ge78ea</bpmn:incoming>
      <bpmn:outgoing>Flow_1sn6vco</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_0ge78ea" name="is_adi_aei_kla_wat==&#39;Y&#39;" sourceRef="Gateway_00ttzeq" targetRef="Activity_14mukh3">
      <bpmn:conditionExpression>is_adi_aei_kla_wat=="Y"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:task id="Activity_1vbqhoc" name="申请者(CP)">
      <bpmn:incoming>Flow_0q96hlv</bpmn:incoming>
      <bpmn:outgoing>Flow_1ov5k40</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_0q96hlv" name="is_adi_aei_kla_cp==&#39;Y&#39;" sourceRef="Gateway_00ttzeq" targetRef="Activity_1vbqhoc">
      <bpmn:conditionExpression>is_adi_aei_kla_cp=="Y"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:sequenceFlow id="Flow_1sn6vco" sourceRef="Activity_14mukh3" targetRef="Gateway_1urue73" />
    <bpmn:sequenceFlow id="Flow_1ov5k40" sourceRef="Activity_1vbqhoc" targetRef="Event_0wl3if9" />
    <bpmn:task id="Activity_15ixilp" name="申请者(KLA)">
      <bpmn:incoming>Flow_1ha7z3f</bpmn:incoming>
      <bpmn:outgoing>Flow_0dfo4q1</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_1ha7z3f" name="adi_kla==&#39;Y&#39;" sourceRef="Gateway_0psd4cv" targetRef="Activity_15ixilp">
      <bpmn:conditionExpression>adi_kla=="Y"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:exclusiveGateway id="Gateway_0aya4i2">
      <bpmn:incoming>Flow_0dfo4q1</bpmn:incoming>
      <bpmn:outgoing>Flow_1k4sajm</bpmn:outgoing>
      <bpmn:outgoing>Flow_05xracx</bpmn:outgoing>
    </bpmn:exclusiveGateway>
    <bpmn:sequenceFlow id="Flow_0dfo4q1" sourceRef="Activity_15ixilp" targetRef="Gateway_0aya4i2" />
    <bpmn:task id="Activity_0js5lna" name="申请者部门经理">
      <bpmn:incoming>Flow_1k4sajm</bpmn:incoming>
      <bpmn:outgoing>Flow_16gw92s</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_1k4sajm" name="kla_case_flag=&#39;Y&#39;" sourceRef="Gateway_0aya4i2" targetRef="Activity_0js5lna">
      <bpmn:conditionExpression>kla_case_flag=="Y"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:sequenceFlow id="Flow_05xracx" name="kla_case_flag=&#39;N&#39;" sourceRef="Gateway_0aya4i2" targetRef="Activity_0ljvldo">
      <bpmn:conditionExpression>kla_case_flag=="N"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:sequenceFlow id="Flow_16gw92s" sourceRef="Activity_0js5lna" targetRef="Activity_0ljvldo" />
    <bpmn:exclusiveGateway id="Gateway_1siijko">
      <bpmn:incoming>Flow_1udmara</bpmn:incoming>
      <bpmn:outgoing>Flow_0eogh1z</bpmn:outgoing>
      <bpmn:outgoing>Flow_0oqekn6</bpmn:outgoing>
    </bpmn:exclusiveGateway>
    <bpmn:sequenceFlow id="Flow_1udmara" sourceRef="StartEvent_1" targetRef="Gateway_1siijko" />
    <bpmn:task id="Activity_03qtx3t" name="部门经理">
      <bpmn:incoming>Flow_0eogh1z</bpmn:incoming>
      <bpmn:outgoing>Flow_1tp1rws</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_0eogh1z" name="adi_case_flag=&#39;Y&#39;" sourceRef="Gateway_1siijko" targetRef="Activity_03qtx3t">
      <bpmn:conditionExpression>adi_case_flag=="Y"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:sequenceFlow id="Flow_0oqekn6" sourceRef="Gateway_1siijko" targetRef="Activity_1bxsgwg" />
    <bpmn:sequenceFlow id="Flow_1tp1rws" sourceRef="Activity_03qtx3t" targetRef="Activity_1bxsgwg" />
    <bpmn:task id="Activity_026lzdh" name="申请者(KLA)部门经理">
      <bpmn:incoming>Flow_0pa2mca</bpmn:incoming>
      <bpmn:outgoing>Flow_15x2z9b</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_15x2z9b" sourceRef="Activity_026lzdh" targetRef="Activity_07w85xm" />
    <bpmn:sequenceFlow id="Flow_0pa2mca" name="adi_aei_kla_case_flag==&#34;Y&#34;" sourceRef="Gateway_0h33o64" targetRef="Activity_026lzdh">
      <bpmn:conditionExpression>adi_aei_kla_case_flag=="Y"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:sequenceFlow id="Flow_1iio5g2" name="adi_aei_kla_case_flag==&#34;N&#34;" sourceRef="Gateway_0h33o64" targetRef="Activity_07w85xm">
      <bpmn:conditionExpression>adi_aei_kla_case_flag=="N"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:sequenceFlow id="Flow_1w9tz8x" sourceRef="Activity_1bivb6l" targetRef="Event_0wl3if9" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_3350">
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="-358" y="802" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="-372" y="848" width="64" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1bxsgwg_di" bpmnElement="Activity_1bxsgwg">
        <dc:Bounds x="-10" y="780" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_00yyue8_di" bpmnElement="Activity_00yyue8">
        <dc:Bounds x="210" y="780" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0gdpp9o_di" bpmnElement="Activity_0gdpp9o">
        <dc:Bounds x="400" y="780" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_1tmkd46_di" bpmnElement="Gateway_1tmkd46" isMarkerVisible="true">
        <dc:Bounds x="645" y="795" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0wl3if9_di" bpmnElement="Event_0wl3if9">
        <dc:Bounds x="3412" y="1322" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="3420" y="1368" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_0psd4cv_di" bpmnElement="Gateway_0psd4cv" isMarkerVisible="true">
        <dc:Bounds x="1035" y="475" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0w3e41w_di" bpmnElement="Activity_0w3e41w">
        <dc:Bounds x="1260" y="20" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_01paanv_di" bpmnElement="Activity_01paanv">
        <dc:Bounds x="1160" y="910" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_08j2a4w_di" bpmnElement="Activity_08j2a4w">
        <dc:Bounds x="1150" y="1180" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_0b55ajv_di" bpmnElement="Gateway_0b55ajv" isMarkerVisible="true">
        <dc:Bounds x="1515" y="35" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_08luf37_di" bpmnElement="Activity_08luf37">
        <dc:Bounds x="1640" y="280" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_0h33o64_di" bpmnElement="Gateway_0h33o64" isMarkerVisible="true">
        <dc:Bounds x="1815" y="295" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_07w85xm_di" bpmnElement="Activity_07w85xm">
        <dc:Bounds x="2000" y="110" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0r32pum_di" bpmnElement="Activity_0r32pum">
        <dc:Bounds x="2000" y="430" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0dku4wx_di" bpmnElement="Activity_0dku4wx">
        <dc:Bounds x="2010" y="550" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0f7u3bd_di" bpmnElement="Activity_0f7u3bd">
        <dc:Bounds x="2150" y="190" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1y36h74_di" bpmnElement="Activity_1y36h74">
        <dc:Bounds x="2320" y="190" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_1urue73_di" bpmnElement="Gateway_1urue73" isMarkerVisible="true">
        <dc:Bounds x="2835" y="125" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1bivb6l_di" bpmnElement="Activity_1bivb6l">
        <dc:Bounds x="3200" y="-10" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_0n0qla0_di" bpmnElement="Gateway_0n0qla0" isMarkerVisible="true">
        <dc:Bounds x="2405" y="445" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1c8y7s3_di" bpmnElement="Activity_1c8y7s3">
        <dc:Bounds x="2980" y="430" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_017w2ns_di" bpmnElement="Activity_017w2ns">
        <dc:Bounds x="1620" y="910" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_0ibh3r4_di" bpmnElement="Gateway_0ibh3r4" isMarkerVisible="true">
        <dc:Bounds x="1805" y="925" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0tw5log_di" bpmnElement="Activity_0tw5log">
        <dc:Bounds x="1970" y="710" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0dk6cem_di" bpmnElement="Activity_0dk6cem">
        <dc:Bounds x="1960" y="910" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1l1pkbd_di" bpmnElement="Activity_1l1pkbd">
        <dc:Bounds x="1970" y="1060" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0ljvldo_di" bpmnElement="Activity_0ljvldo">
        <dc:Bounds x="1620" y="610" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0uyucp2_di" bpmnElement="Activity_0uyucp2">
        <dc:Bounds x="1620" y="740" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_04md141_di" bpmnElement="Gateway_04md141" isMarkerVisible="true">
        <dc:Bounds x="2355" y="715" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_16sqpei_di" bpmnElement="Activity_16sqpei">
        <dc:Bounds x="2420" y="700" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_1vzgw1q_di" bpmnElement="Gateway_1vzgw1q" isMarkerVisible="true">
        <dc:Bounds x="2555" y="715" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1mrdlz0_di" bpmnElement="Activity_1mrdlz0">
        <dc:Bounds x="2640" y="700" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_1f38fy0_di" bpmnElement="Gateway_1f38fy0" isMarkerVisible="true">
        <dc:Bounds x="2135" y="825" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_12qzdzn_di" bpmnElement="Activity_12qzdzn">
        <dc:Bounds x="2250" y="810" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_1rfs9b7_di" bpmnElement="Gateway_1rfs9b7" isMarkerVisible="true">
        <dc:Bounds x="1305" y="925" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1qcdlr8_di" bpmnElement="Activity_1qcdlr8">
        <dc:Bounds x="1400" y="910" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_00ttzeq_di" bpmnElement="Gateway_00ttzeq" isMarkerVisible="true">
        <dc:Bounds x="2495" y="205" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_14mukh3_di" bpmnElement="Activity_14mukh3">
        <dc:Bounds x="2610" y="120" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1vbqhoc_di" bpmnElement="Activity_1vbqhoc">
        <dc:Bounds x="2600" y="280" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_15ixilp_di" bpmnElement="Activity_15ixilp">
        <dc:Bounds x="1220" y="460" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_0aya4i2_di" bpmnElement="Gateway_0aya4i2" isMarkerVisible="true">
        <dc:Bounds x="1445" y="475" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0js5lna_di" bpmnElement="Activity_0js5lna">
        <dc:Bounds x="1620" y="460" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_1siijko_di" bpmnElement="Gateway_1siijko" isMarkerVisible="true">
        <dc:Bounds x="-235" y="795" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_03qtx3t_di" bpmnElement="Activity_03qtx3t">
        <dc:Bounds x="-140" y="690" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_026lzdh_di" bpmnElement="Activity_026lzdh">
        <dc:Bounds x="2000" y="-60" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_0dicvil_di" bpmnElement="Flow_0dicvil">
        <di:waypoint x="310" y="820" />
        <di:waypoint x="400" y="820" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_059a31g_di" bpmnElement="Flow_059a31g">
        <di:waypoint x="500" y="820" />
        <di:waypoint x="645" y="820" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1jsdadk_di" bpmnElement="Flow_1jsdadk">
        <di:waypoint x="670" y="845" />
        <di:waypoint x="670" y="1340" />
        <di:waypoint x="3412" y="1340" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="948" y="1313" width="84" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1xstu9n_di" bpmnElement="Flow_1xstu9n">
        <di:waypoint x="670" y="795" />
        <di:waypoint x="670" y="500" />
        <di:waypoint x="1035" y="500" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="807" y="480" width="85" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0gbmu89_di" bpmnElement="Flow_0gbmu89">
        <di:waypoint x="1060" y="475" />
        <di:waypoint x="1060" y="60" />
        <di:waypoint x="1260" y="60" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1138" y="33" width="60" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_19ty7g4_di" bpmnElement="Flow_19ty7g4">
        <di:waypoint x="1060" y="525" />
        <di:waypoint x="1060" y="950" />
        <di:waypoint x="1160" y="950" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1068" y="713" width="63" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1dz4ops_di" bpmnElement="Flow_1dz4ops">
        <di:waypoint x="1060" y="525" />
        <di:waypoint x="1060" y="1220" />
        <di:waypoint x="1150" y="1220" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1071" y="1194" width="57" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1elwptq_di" bpmnElement="Flow_1elwptq">
        <di:waypoint x="1360" y="60" />
        <di:waypoint x="1515" y="60" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0jgw3lc_di" bpmnElement="Flow_0jgw3lc">
        <di:waypoint x="1540" y="85" />
        <di:waypoint x="1540" y="310" />
        <di:waypoint x="1640" y="310" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1abqg2h_di" bpmnElement="Flow_1abqg2h">
        <di:waypoint x="1540" y="35" />
        <di:waypoint x="1540" y="-110" />
        <di:waypoint x="3430" y="-110" />
        <di:waypoint x="3430" y="1322" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="2493" y="-134" width="87" height="27" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_003pgvq_di" bpmnElement="Flow_003pgvq">
        <di:waypoint x="1740" y="320" />
        <di:waypoint x="1815" y="320" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1az2998_di" bpmnElement="Flow_1az2998">
        <di:waypoint x="1840" y="345" />
        <di:waypoint x="1840" y="470" />
        <di:waypoint x="2000" y="470" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1872" y="450" width="84" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1fpxn4w_di" bpmnElement="Flow_1fpxn4w">
        <di:waypoint x="1840" y="345" />
        <di:waypoint x="1840" y="590" />
        <di:waypoint x="2010" y="590" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1880" y="563" width="78" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0g8rhrk_di" bpmnElement="Flow_0g8rhrk">
        <di:waypoint x="2050" y="190" />
        <di:waypoint x="2050" y="230" />
        <di:waypoint x="2150" y="230" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0n9sd9r_di" bpmnElement="Flow_0n9sd9r">
        <di:waypoint x="2250" y="230" />
        <di:waypoint x="2320" y="230" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1tfd8ic_di" bpmnElement="Flow_1tfd8ic">
        <di:waypoint x="2885" y="150" />
        <di:waypoint x="3250" y="150" />
        <di:waypoint x="3250" y="1340" />
        <di:waypoint x="3412" y="1340" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="3199" y="676" width="82" height="27" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0345qun_di" bpmnElement="Flow_0345qun">
        <di:waypoint x="2860" y="125" />
        <di:waypoint x="2860" y="10" />
        <di:waypoint x="3200" y="10" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="2798" y="62" width="84" height="27" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0gazemr_di" bpmnElement="Flow_0gazemr">
        <di:waypoint x="2110" y="590" />
        <di:waypoint x="2840" y="590" />
        <di:waypoint x="2840" y="1340" />
        <di:waypoint x="3412" y="1340" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1pkxdjz_di" bpmnElement="Flow_1pkxdjz">
        <di:waypoint x="2100" y="470" />
        <di:waypoint x="2405" y="470" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_01pulbz_di" bpmnElement="Flow_01pulbz">
        <di:waypoint x="2455" y="470" />
        <di:waypoint x="2980" y="470" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1es7c32_di" bpmnElement="Flow_1es7c32">
        <di:waypoint x="2455" y="470" />
        <di:waypoint x="2930" y="470" />
        <di:waypoint x="2930" y="1340" />
        <di:waypoint x="3412" y="1340" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="2897" y="876" width="86" height="27" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1mx5loj_di" bpmnElement="Flow_1mx5loj">
        <di:waypoint x="3030" y="510" />
        <di:waypoint x="3030" y="1340" />
        <di:waypoint x="3412" y="1340" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0ws7ij4_di" bpmnElement="Flow_0ws7ij4">
        <di:waypoint x="1200" y="1260" />
        <di:waypoint x="1200" y="1340" />
        <di:waypoint x="3412" y="1340" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_06f13zd_di" bpmnElement="Flow_06f13zd">
        <di:waypoint x="1720" y="950" />
        <di:waypoint x="1805" y="950" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0u9hvox_di" bpmnElement="Flow_0u9hvox">
        <di:waypoint x="1830" y="925" />
        <di:waypoint x="1830" y="750" />
        <di:waypoint x="1970" y="750" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1867" y="723" width="81" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_19zvk4a_di" bpmnElement="Flow_19zvk4a">
        <di:waypoint x="1855" y="950" />
        <di:waypoint x="1960" y="950" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1866" y="956" width="83" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1s2e7oq_di" bpmnElement="Flow_1s2e7oq">
        <di:waypoint x="1830" y="975" />
        <di:waypoint x="1830" y="1100" />
        <di:waypoint x="1970" y="1100" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1870" y="1073" width="77" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0dyyinx_di" bpmnElement="Flow_0dyyinx">
        <di:waypoint x="90" y="820" />
        <di:waypoint x="210" y="820" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0w9uvsj_di" bpmnElement="Flow_0w9uvsj">
        <di:waypoint x="1670" y="690" />
        <di:waypoint x="1670" y="740" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0y41dvf_di" bpmnElement="Flow_0y41dvf">
        <di:waypoint x="1670" y="820" />
        <di:waypoint x="1670" y="910" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0zr7cde_di" bpmnElement="Flow_0zr7cde">
        <di:waypoint x="2070" y="740" />
        <di:waypoint x="2355" y="740" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1ivg1nv_di" bpmnElement="Flow_1ivg1nv">
        <di:waypoint x="2405" y="740" />
        <di:waypoint x="2420" y="740" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_13h3eu2_di" bpmnElement="Flow_13h3eu2">
        <di:waypoint x="2380" y="765" />
        <di:waypoint x="2380" y="1340" />
        <di:waypoint x="3412" y="1340" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="2366" y="1050" width="82" height="27" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0lp6ozk_di" bpmnElement="Flow_0lp6ozk">
        <di:waypoint x="2520" y="740" />
        <di:waypoint x="2555" y="740" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_06efhc0_di" bpmnElement="Flow_06efhc0">
        <di:waypoint x="2605" y="740" />
        <di:waypoint x="2640" y="740" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0ktpapa_di" bpmnElement="Flow_0ktpapa">
        <di:waypoint x="2580" y="765" />
        <di:waypoint x="2580" y="1340" />
        <di:waypoint x="3412" y="1340" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="2564" y="1050" width="84" height="27" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0x9fkpe_di" bpmnElement="Flow_0x9fkpe">
        <di:waypoint x="2690" y="780" />
        <di:waypoint x="2690" y="1340" />
        <di:waypoint x="3412" y="1340" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_00okasn_di" bpmnElement="Flow_00okasn">
        <di:waypoint x="2020" y="1140" />
        <di:waypoint x="2020" y="1340" />
        <di:waypoint x="3412" y="1340" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1j2620d_di" bpmnElement="Flow_1j2620d">
        <di:waypoint x="2010" y="910" />
        <di:waypoint x="2010" y="850" />
        <di:waypoint x="2135" y="850" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1wpqqls_di" bpmnElement="Flow_1wpqqls">
        <di:waypoint x="2185" y="850" />
        <di:waypoint x="2250" y="850" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0rr7zbp_di" bpmnElement="Flow_0rr7zbp">
        <di:waypoint x="2300" y="890" />
        <di:waypoint x="2300" y="1340" />
        <di:waypoint x="3412" y="1340" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1h7yjfz_di" bpmnElement="Flow_1h7yjfz">
        <di:waypoint x="1260" y="950" />
        <di:waypoint x="1305" y="950" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1swtbm9_di" bpmnElement="Flow_1swtbm9">
        <di:waypoint x="1355" y="950" />
        <di:waypoint x="1400" y="950" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0heysg2_di" bpmnElement="Flow_0heysg2">
        <di:waypoint x="1330" y="975" />
        <di:waypoint x="1330" y="1340" />
        <di:waypoint x="3412" y="1340" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1310" y="1155" width="89" height="27" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0hcihmf_di" bpmnElement="Flow_0hcihmf">
        <di:waypoint x="1430" y="990" />
        <di:waypoint x="1430" y="1340" />
        <di:waypoint x="3412" y="1340" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1fljcjd_di" bpmnElement="Flow_1fljcjd">
        <di:waypoint x="1830" y="975" />
        <di:waypoint x="1830" y="1340" />
        <di:waypoint x="3412" y="1340" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1812" y="1152" width="86" height="27" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1gnfpjw_di" bpmnElement="Flow_1gnfpjw">
        <di:waypoint x="2160" y="875" />
        <di:waypoint x="2160" y="1340" />
        <di:waypoint x="3412" y="1340" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="2144" y="1105" width="85" height="27" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_127f5oi_di" bpmnElement="Flow_127f5oi">
        <di:waypoint x="2420" y="230" />
        <di:waypoint x="2495" y="230" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0ge78ea_di" bpmnElement="Flow_0ge78ea">
        <di:waypoint x="2520" y="205" />
        <di:waypoint x="2520" y="160" />
        <di:waypoint x="2610" y="160" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="2511" y="136" width="90" height="27" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0q96hlv_di" bpmnElement="Flow_0q96hlv">
        <di:waypoint x="2520" y="255" />
        <di:waypoint x="2520" y="320" />
        <di:waypoint x="2600" y="320" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="2514" y="294" width="88" height="27" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1sn6vco_di" bpmnElement="Flow_1sn6vco">
        <di:waypoint x="2710" y="150" />
        <di:waypoint x="2835" y="150" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1ov5k40_di" bpmnElement="Flow_1ov5k40">
        <di:waypoint x="2700" y="320" />
        <di:waypoint x="3170" y="320" />
        <di:waypoint x="3170" y="1340" />
        <di:waypoint x="3412" y="1340" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1ha7z3f_di" bpmnElement="Flow_1ha7z3f">
        <di:waypoint x="1085" y="500" />
        <di:waypoint x="1220" y="500" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1110" y="513" width="60" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0dfo4q1_di" bpmnElement="Flow_0dfo4q1">
        <di:waypoint x="1320" y="500" />
        <di:waypoint x="1445" y="500" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1k4sajm_di" bpmnElement="Flow_1k4sajm">
        <di:waypoint x="1495" y="500" />
        <di:waypoint x="1620" y="500" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1517" y="473" width="86" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_05xracx_di" bpmnElement="Flow_05xracx">
        <di:waypoint x="1470" y="525" />
        <di:waypoint x="1470" y="630" />
        <di:waypoint x="1620" y="630" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1487" y="603" width="86" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_16gw92s_di" bpmnElement="Flow_16gw92s">
        <di:waypoint x="1670" y="540" />
        <di:waypoint x="1670" y="610" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1udmara_di" bpmnElement="Flow_1udmara">
        <di:waypoint x="-322" y="820" />
        <di:waypoint x="-235" y="820" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0eogh1z_di" bpmnElement="Flow_0eogh1z">
        <di:waypoint x="-210" y="795" />
        <di:waypoint x="-210" y="730" />
        <di:waypoint x="-140" y="730" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="-238" y="761" width="86" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0oqekn6_di" bpmnElement="Flow_0oqekn6">
        <di:waypoint x="-185" y="820" />
        <di:waypoint x="-10" y="820" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1tp1rws_di" bpmnElement="Flow_1tp1rws">
        <di:waypoint x="-40" y="720" />
        <di:waypoint x="40" y="720" />
        <di:waypoint x="40" y="780" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_15x2z9b_di" bpmnElement="Flow_15x2z9b">
        <di:waypoint x="2050" y="20" />
        <di:waypoint x="2050" y="110" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0pa2mca_di" bpmnElement="Flow_0pa2mca">
        <di:waypoint x="1840" y="295" />
        <di:waypoint x="1840" y="-20" />
        <di:waypoint x="2000" y="-20" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1887" y="-14" width="86" height="27" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1iio5g2_di" bpmnElement="Flow_1iio5g2">
        <di:waypoint x="1840" y="295" />
        <di:waypoint x="1840" y="150" />
        <di:waypoint x="2000" y="150" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1887" y="166" width="86" height="27" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1w9tz8x_di" bpmnElement="Flow_1w9tz8x">
        <di:waypoint x="3300" y="30" />
        <di:waypoint x="3340" y="30" />
        <di:waypoint x="3340" y="1340" />
        <di:waypoint x="3412" y="1340" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
', 0, NULL, 0, NULL, NULL, NULL, '2025-09-08 17:14:21+08', '2025-09-08 20:47:35.019+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (24, 'Process_1621', '1', 'Process_Name_1621', 'Process_Code_1621', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_1621" sf:code="Process_Code_1621" name="Process_Name_1621" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="StartEvent_1" name="Start">
      <bpmn:outgoing>Flow_0h3md70</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:task id="Activity_18959ra" name="submit">
      <bpmn:extensionElements>
        <sf:sections>
          <sf:section name="myProperties">{"url": "www.slickflow.com"}</sf:section>
        </sf:sections>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_0h3md70</bpmn:incoming>
      <bpmn:outgoing>Flow_014popf</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_0h3md70" sourceRef="StartEvent_1" targetRef="Activity_18959ra" />
    <bpmn:endEvent id="Event_0y66hzb" name="End">
      <bpmn:incoming>Flow_1iqbiak</bpmn:incoming>
    </bpmn:endEvent>
    <bpmn:serviceTask id="Activity_1t8rtxz" name="llm service">
      <bpmn:extensionElements>
        <sf:aIServices>
          <sf:aIService configUUID="32211f70-0e75-48a3-e001-eb3bfed2d913" type="LLM" />
        </sf:aIServices>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_014popf</bpmn:incoming>
      <bpmn:outgoing>Flow_1iqbiak</bpmn:outgoing>
    </bpmn:serviceTask>
    <bpmn:sequenceFlow id="Flow_014popf" sourceRef="Activity_18959ra" targetRef="Activity_1t8rtxz" />
    <bpmn:sequenceFlow id="Flow_1iqbiak" sourceRef="Activity_1t8rtxz" targetRef="Event_0y66hzb" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_1621">
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_18959ra_di" bpmnElement="Activity_18959ra">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0y66hzb_di" bpmnElement="Event_0y66hzb">
        <dc:Bounds x="902" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="910" y="283" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1t8rtxz_di" bpmnElement="Activity_1t8rtxz">
        <dc:Bounds x="670" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_0h3md70_di" bpmnElement="Flow_0h3md70">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_014popf_di" bpmnElement="Flow_014popf">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="670" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1iqbiak_di" bpmnElement="Flow_1iqbiak">
        <di:waypoint x="770" y="258" />
        <di:waypoint x="902" y="258" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          ', 0, NULL, 0, NULL, NULL, NULL, '2025-11-05 14:29:20+08', '2025-11-12 12:22:35.82+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (27, 'Process_AircraftManufacturing_m4up', '1', '飞机制造流程', 'Process_AircraftManufacturing_m4up', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="Process_AircraftManufacturing_m4up" name="飞机制造流程" isExecutable="false"><bpmn:startEvent id="AStartEvent" name="开始" /><bpmn:userTask id="AActivity_RequirementsAnalysis" name="需求分析与设计规划" /><bpmn:userTask id="AActivity_Design" name="飞机结构与系统设计" /><bpmn:serviceTask id="AActivity_SimulationValidation" name="仿真与设计验证" /><bpmn:exclusiveGateway id="AGateway_DesignApproved" name="设计是否通过？" /><bpmn:userTask id="AActivity_ComponentManufacturing" name="零部件制造" /><bpmn:userTask id="AActivity_Assembly" name="总装集成" /><bpmn:userTask id="AActivity_QualityInspection" name="质量检测与测试" /><bpmn:exclusiveGateway id="AGateway_PassInspection" name="检测是否通过？" /><bpmn:userTask id="AActivity_Correction" name="问题修复与调整" /><bpmn:userTask id="AActivity_FinalDelivery" name="最终交付与客户验收" /><bpmn:endEvent id="AEndEvent_Success" name="结束" /><bpmn:sequenceFlow id="Flow_AStartEvent_AActivity_RequirementsAnalysis" sourceRef="AStartEvent" targetRef="AActivity_RequirementsAnalysis" /><bpmn:sequenceFlow id="Flow_AActivity_RequirementsAnalysis_AActivity_Design" sourceRef="AActivity_RequirementsAnalysis" targetRef="AActivity_Design" /><bpmn:sequenceFlow id="Flow_AActivity_Design_AActivity_SimulationValidation" sourceRef="AActivity_Design" targetRef="AActivity_SimulationValidation" /><bpmn:sequenceFlow id="Flow_AActivity_SimulationValidation_AGateway_DesignApproved" sourceRef="AActivity_SimulationValidation" targetRef="AGateway_DesignApproved" /><bpmn:sequenceFlow id="Flow_AGateway_DesignApproved_AActivity_ComponentManufacturing" sourceRef="AGateway_DesignApproved" targetRef="AActivity_ComponentManufacturing"><bpmn:conditionExpression>设计通过</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_AGateway_DesignApproved_AActivity_Design" sourceRef="AGateway_DesignApproved" targetRef="AActivity_Design"><bpmn:conditionExpression>设计未通过</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_AActivity_ComponentManufacturing_AActivity_Assembly" sourceRef="AActivity_ComponentManufacturing" targetRef="AActivity_Assembly" /><bpmn:sequenceFlow id="Flow_AActivity_Assembly_AActivity_QualityInspection" sourceRef="AActivity_Assembly" targetRef="AActivity_QualityInspection" /><bpmn:sequenceFlow id="Flow_AActivity_QualityInspection_AGateway_PassInspection" sourceRef="AActivity_QualityInspection" targetRef="AGateway_PassInspection" /><bpmn:sequenceFlow id="Flow_AGateway_PassInspection_AActivity_FinalDelivery" sourceRef="AGateway_PassInspection" targetRef="AActivity_FinalDelivery"><bpmn:conditionExpression>检测通过</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_AGateway_PassInspection_AActivity_Correction" sourceRef="AGateway_PassInspection" targetRef="AActivity_Correction"><bpmn:conditionExpression>检测未通过</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_AActivity_Correction_AActivity_QualityInspection" sourceRef="AActivity_Correction" targetRef="AActivity_QualityInspection" /><bpmn:sequenceFlow id="Flow_AActivity_FinalDelivery_AEndEvent_Success" sourceRef="AActivity_FinalDelivery" targetRef="AEndEvent_Success" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_AircraftManufacturing_m4up"><bpmndi:BPMNShape id="Shape_AStartEvent" bpmnElement="AStartEvent"><dc:Bounds x="150" y="162" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_RequirementsAnalysis" bpmnElement="AActivity_RequirementsAnalysis"><dc:Bounds x="350" y="140" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_Design" bpmnElement="AActivity_Design"><dc:Bounds x="550" y="140" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_SimulationValidation" bpmnElement="AActivity_SimulationValidation"><dc:Bounds x="750" y="261" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AGateway_DesignApproved" bpmnElement="AGateway_DesignApproved"><dc:Bounds x="950" y="140" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_ComponentManufacturing" bpmnElement="AActivity_ComponentManufacturing"><dc:Bounds x="1150" y="140" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_Assembly" bpmnElement="AActivity_Assembly"><dc:Bounds x="1350" y="140" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_QualityInspection" bpmnElement="AActivity_QualityInspection"><dc:Bounds x="1550" y="140" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AGateway_PassInspection" bpmnElement="AGateway_PassInspection"><dc:Bounds x="1750" y="261" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_Correction" bpmnElement="AActivity_Correction"><dc:Bounds x="1950" y="140" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_FinalDelivery" bpmnElement="AActivity_FinalDelivery"><dc:Bounds x="1950" y="301" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent_Success" bpmnElement="AEndEvent_Success"><dc:Bounds x="2150" y="323" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AStartEvent_AActivity_RequirementsAnalysis" bpmnElement="Flow_AStartEvent_AActivity_RequirementsAnalysis"><di:waypoint x="186" y="180" /><di:waypoint x="236" y="180" /><di:waypoint x="236" y="180" /><di:waypoint x="350" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_RequirementsAnalysis_AActivity_Design" bpmnElement="Flow_AActivity_RequirementsAnalysis_AActivity_Design"><di:waypoint x="450" y="180" /><di:waypoint x="500" y="180" /><di:waypoint x="550" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_Design_AActivity_SimulationValidation" bpmnElement="Flow_AActivity_Design_AActivity_SimulationValidation"><di:waypoint x="650" y="180" /><di:waypoint x="700" y="180" /><di:waypoint x="700" y="301" /><di:waypoint x="750" y="301" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_SimulationValidation_AGateway_DesignApproved" bpmnElement="Flow_AActivity_SimulationValidation_AGateway_DesignApproved"><di:waypoint x="850" y="301" /><di:waypoint x="900" y="301" /><di:waypoint x="900" y="158" /><di:waypoint x="950" y="158" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_DesignApproved_AActivity_ComponentManufacturing" bpmnElement="Flow_AGateway_DesignApproved_AActivity_ComponentManufacturing"><di:waypoint x="986" y="158" /><di:waypoint x="1068" y="158" /><di:waypoint x="1150" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_DesignApproved_AActivity_Design" bpmnElement="Flow_AGateway_DesignApproved_AActivity_Design"><di:waypoint x="986" y="158" /><di:waypoint x="768" y="158" /><di:waypoint x="550" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_ComponentManufacturing_AActivity_Assembly" bpmnElement="Flow_AActivity_ComponentManufacturing_AActivity_Assembly"><di:waypoint x="1250" y="180" /><di:waypoint x="1300" y="180" /><di:waypoint x="1350" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_Assembly_AActivity_QualityInspection" bpmnElement="Flow_AActivity_Assembly_AActivity_QualityInspection"><di:waypoint x="1450" y="180" /><di:waypoint x="1500" y="180" /><di:waypoint x="1550" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_QualityInspection_AGateway_PassInspection" bpmnElement="Flow_AActivity_QualityInspection_AGateway_PassInspection"><di:waypoint x="1650" y="180" /><di:waypoint x="1700" y="180" /><di:waypoint x="1700" y="279" /><di:waypoint x="1750" y="279" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_PassInspection_AActivity_FinalDelivery" bpmnElement="Flow_AGateway_PassInspection_AActivity_FinalDelivery"><di:waypoint x="1786" y="279" /><di:waypoint x="1836" y="279" /><di:waypoint x="1836" y="341" /><di:waypoint x="1950" y="341" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_PassInspection_AActivity_Correction" bpmnElement="Flow_AGateway_PassInspection_AActivity_Correction"><di:waypoint x="1786" y="279" /><di:waypoint x="1836" y="279" /><di:waypoint x="1836" y="180" /><di:waypoint x="1950" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_Correction_AActivity_QualityInspection" bpmnElement="Flow_AActivity_Correction_AActivity_QualityInspection"><di:waypoint x="2050" y="180" /><di:waypoint x="1800" y="180" /><di:waypoint x="1550" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_FinalDelivery_AEndEvent_Success" bpmnElement="Flow_AActivity_FinalDelivery_AEndEvent_Success"><di:waypoint x="2050" y="341" /><di:waypoint x="2100" y="341" /><di:waypoint x="2100" y="341" /><di:waypoint x="2150" y="341" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, 0, NULL, NULL, NULL, '2025-12-03 22:00:19.76574+08', NULL, NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (28, 'Process_PublicInstitutionRecruitment_qtd7', '1', '事业单位招聘流程', 'Process_PublicInstitutionRecruitment_qtd7', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="Process_PublicInstitutionRecruitment_qtd7" name="事业单位招聘流程" isExecutable="false"><bpmn:startEvent id="AStartEvent" name="开始" /><bpmn:userTask id="AActivity_PlanApproval" name="招聘计划申报与审批" /><bpmn:userTask id="AActivity_PostAnnouncement" name="发布招聘公告" /><bpmn:userTask id="AActivity_Application" name="应聘人员报名" /><bpmn:userTask id="AActivity_QualificationReview" name="资格审查" /><bpmn:exclusiveGateway id="AGateway_QualificationPassed" name="资格审查通过？" /><bpmn:userTask id="AActivity_WrittenExam" name="组织笔试" /><bpmn:userTask id="AActivity_Interview" name="组织面试" /><bpmn:userTask id="AActivity_MedicalCheckup" name="体检" /><bpmn:userTask id="AActivity_PoliticalReview" name="政审与背景调查" /><bpmn:userTask id="AActivity_Publicity" name="公示拟录用人员" /><bpmn:exclusiveGateway id="AGateway_ApprovalPassed" name="公示无异议？" /><bpmn:userTask id="AActivity_IssueOffer" name="发放录用通知" /><bpmn:endEvent id="AEndEvent_Success" name="结束（招聘完成）" /><bpmn:endEvent id="AEndEvent_Reject" name="结束（未通过）" /><bpmn:sequenceFlow id="Flow_AStartEvent_AActivity_PlanApproval" sourceRef="AStartEvent" targetRef="AActivity_PlanApproval" /><bpmn:sequenceFlow id="Flow_AActivity_PlanApproval_AActivity_PostAnnouncement" sourceRef="AActivity_PlanApproval" targetRef="AActivity_PostAnnouncement" /><bpmn:sequenceFlow id="Flow_AActivity_PostAnnouncement_AActivity_Application" sourceRef="AActivity_PostAnnouncement" targetRef="AActivity_Application" /><bpmn:sequenceFlow id="Flow_AActivity_Application_AActivity_QualificationReview" sourceRef="AActivity_Application" targetRef="AActivity_QualificationReview" /><bpmn:sequenceFlow id="Flow_AActivity_QualificationReview_AGateway_QualificationPassed" sourceRef="AActivity_QualificationReview" targetRef="AGateway_QualificationPassed" /><bpmn:sequenceFlow id="Flow_AGateway_QualificationPassed_AActivity_WrittenExam" sourceRef="AGateway_QualificationPassed" targetRef="AActivity_WrittenExam" /><bpmn:sequenceFlow id="Flow_AGateway_QualificationPassed_AEndEvent_Reject" sourceRef="AGateway_QualificationPassed" targetRef="AEndEvent_Reject" /><bpmn:sequenceFlow id="Flow_AActivity_WrittenExam_AActivity_Interview" sourceRef="AActivity_WrittenExam" targetRef="AActivity_Interview" /><bpmn:sequenceFlow id="Flow_AActivity_Interview_AActivity_MedicalCheckup" sourceRef="AActivity_Interview" targetRef="AActivity_MedicalCheckup" /><bpmn:sequenceFlow id="Flow_AActivity_MedicalCheckup_AActivity_PoliticalReview" sourceRef="AActivity_MedicalCheckup" targetRef="AActivity_PoliticalReview" /><bpmn:sequenceFlow id="Flow_AActivity_PoliticalReview_AActivity_Publicity" sourceRef="AActivity_PoliticalReview" targetRef="AActivity_Publicity" /><bpmn:sequenceFlow id="Flow_AActivity_Publicity_AGateway_ApprovalPassed" sourceRef="AActivity_Publicity" targetRef="AGateway_ApprovalPassed" /><bpmn:sequenceFlow id="Flow_AGateway_ApprovalPassed_AActivity_IssueOffer" sourceRef="AGateway_ApprovalPassed" targetRef="AActivity_IssueOffer" /><bpmn:sequenceFlow id="Flow_AGateway_ApprovalPassed_AEndEvent_Reject" sourceRef="AGateway_ApprovalPassed" targetRef="AEndEvent_Reject" /><bpmn:sequenceFlow id="Flow_AActivity_IssueOffer_AEndEvent_Success" sourceRef="AActivity_IssueOffer" targetRef="AEndEvent_Success" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_PublicInstitutionRecruitment_qtd7"><bpmndi:BPMNShape id="Shape_AStartEvent" bpmnElement="AStartEvent"><dc:Bounds x="150" y="162" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_PlanApproval" bpmnElement="AActivity_PlanApproval"><dc:Bounds x="350" y="140" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_PostAnnouncement" bpmnElement="AActivity_PostAnnouncement"><dc:Bounds x="550" y="140" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_Application" bpmnElement="AActivity_Application"><dc:Bounds x="750" y="140" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_QualificationReview" bpmnElement="AActivity_QualificationReview"><dc:Bounds x="950" y="140" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AGateway_QualificationPassed" bpmnElement="AGateway_QualificationPassed"><dc:Bounds x="1150" y="162" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_WrittenExam" bpmnElement="AActivity_WrittenExam"><dc:Bounds x="1350" y="261" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_Interview" bpmnElement="AActivity_Interview"><dc:Bounds x="1550" y="261" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_MedicalCheckup" bpmnElement="AActivity_MedicalCheckup"><dc:Bounds x="1750" y="261" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_PoliticalReview" bpmnElement="AActivity_PoliticalReview"><dc:Bounds x="1950" y="261" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_Publicity" bpmnElement="AActivity_Publicity"><dc:Bounds x="2150" y="261" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AGateway_ApprovalPassed" bpmnElement="AGateway_ApprovalPassed"><dc:Bounds x="2350" y="283" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_IssueOffer" bpmnElement="AActivity_IssueOffer"><dc:Bounds x="2550" y="301" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent_Success" bpmnElement="AEndEvent_Success"><dc:Bounds x="2750" y="323" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent_Reject" bpmnElement="AEndEvent_Reject"><dc:Bounds x="2550" y="140" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AStartEvent_AActivity_PlanApproval" bpmnElement="Flow_AStartEvent_AActivity_PlanApproval"><di:waypoint x="186" y="180" /><di:waypoint x="236" y="180" /><di:waypoint x="236" y="180" /><di:waypoint x="350" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_PlanApproval_AActivity_PostAnnouncement" bpmnElement="Flow_AActivity_PlanApproval_AActivity_PostAnnouncement"><di:waypoint x="450" y="180" /><di:waypoint x="500" y="180" /><di:waypoint x="550" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_PostAnnouncement_AActivity_Application" bpmnElement="Flow_AActivity_PostAnnouncement_AActivity_Application"><di:waypoint x="650" y="180" /><di:waypoint x="700" y="180" /><di:waypoint x="750" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_Application_AActivity_QualificationReview" bpmnElement="Flow_AActivity_Application_AActivity_QualificationReview"><di:waypoint x="850" y="180" /><di:waypoint x="900" y="180" /><di:waypoint x="950" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_QualificationReview_AGateway_QualificationPassed" bpmnElement="Flow_AActivity_QualificationReview_AGateway_QualificationPassed"><di:waypoint x="1050" y="180" /><di:waypoint x="1100" y="180" /><di:waypoint x="1100" y="180" /><di:waypoint x="1150" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_QualificationPassed_AActivity_WrittenExam" bpmnElement="Flow_AGateway_QualificationPassed_AActivity_WrittenExam"><di:waypoint x="1186" y="180" /><di:waypoint x="1236" y="180" /><di:waypoint x="1236" y="301" /><di:waypoint x="1350" y="301" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_QualificationPassed_AEndEvent_Reject" bpmnElement="Flow_AGateway_QualificationPassed_AEndEvent_Reject"><di:waypoint x="1186" y="180" /><di:waypoint x="1236" y="180" /><di:waypoint x="1236" y="158" /><di:waypoint x="2550" y="158" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_WrittenExam_AActivity_Interview" bpmnElement="Flow_AActivity_WrittenExam_AActivity_Interview"><di:waypoint x="1450" y="301" /><di:waypoint x="1500" y="301" /><di:waypoint x="1550" y="301" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_Interview_AActivity_MedicalCheckup" bpmnElement="Flow_AActivity_Interview_AActivity_MedicalCheckup"><di:waypoint x="1650" y="301" /><di:waypoint x="1700" y="301" /><di:waypoint x="1750" y="301" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_MedicalCheckup_AActivity_PoliticalReview" bpmnElement="Flow_AActivity_MedicalCheckup_AActivity_PoliticalReview"><di:waypoint x="1850" y="301" /><di:waypoint x="1900" y="301" /><di:waypoint x="1950" y="301" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_PoliticalReview_AActivity_Publicity" bpmnElement="Flow_AActivity_PoliticalReview_AActivity_Publicity"><di:waypoint x="2050" y="301" /><di:waypoint x="2100" y="301" /><di:waypoint x="2150" y="301" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_Publicity_AGateway_ApprovalPassed" bpmnElement="Flow_AActivity_Publicity_AGateway_ApprovalPassed"><di:waypoint x="2250" y="301" /><di:waypoint x="2300" y="301" /><di:waypoint x="2300" y="301" /><di:waypoint x="2350" y="301" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_ApprovalPassed_AActivity_IssueOffer" bpmnElement="Flow_AGateway_ApprovalPassed_AActivity_IssueOffer"><di:waypoint x="2386" y="301" /><di:waypoint x="2436" y="301" /><di:waypoint x="2436" y="341" /><di:waypoint x="2550" y="341" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_ApprovalPassed_AEndEvent_Reject" bpmnElement="Flow_AGateway_ApprovalPassed_AEndEvent_Reject"><di:waypoint x="2386" y="301" /><di:waypoint x="2436" y="301" /><di:waypoint x="2436" y="158" /><di:waypoint x="2550" y="158" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_IssueOffer_AEndEvent_Success" bpmnElement="Flow_AActivity_IssueOffer_AEndEvent_Success"><di:waypoint x="2650" y="341" /><di:waypoint x="2700" y="341" /><di:waypoint x="2700" y="341" /><di:waypoint x="2750" y="341" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, 0, NULL, NULL, NULL, '2025-12-03 22:03:17.286716+08', NULL, NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (30, 'Process_ImageClassify', '1', 'Pet_Plan_LLM_0901', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" id="Definitions_ImageClassify" targetNamespace="http://bpmn.io/schema/bpmn">
  <bpmn:process id="Process_ImageClassify" name="Pet_Plan_LLM_0901" isExecutable="false">
    <bpmn:startEvent id="StartEvent_1" name="Start" />
    <bpmn:userTask id="Task_UploadImage" name="Upload Image" />
    <bpmn:serviceTask id="Task_ClassifyImage" name="Image Classify (AI)" />
    <bpmn:exclusiveGateway id="Gateway_ClassResult" name="Cat or Dog?" />
    <bpmn:serviceTask id="Task_CatPlan" name="Generate Cat Care Plan" />
    <bpmn:serviceTask id="Task_DogStores" name="Get Dog Store Locations" />
    <bpmn:exclusiveGateway id="Gateway_Merge" name="Merge" gatewayDirection="Converging" />
    <bpmn:endEvent id="EndEvent_1" name="End" />
    <bpmn:sequenceFlow id="Flow_Start_Upload" sourceRef="StartEvent_1" targetRef="Task_UploadImage" />
    <bpmn:sequenceFlow id="Flow_Upload_Classify" sourceRef="Task_UploadImage" targetRef="Task_ClassifyImage" />
    <bpmn:sequenceFlow id="Flow_Classify_Gateway" sourceRef="Task_ClassifyImage" targetRef="Gateway_ClassResult" />
    <bpmn:sequenceFlow id="Flow_Gateway_Cat" name="cat" sourceRef="Gateway_ClassResult" targetRef="Task_CatPlan" />
    <bpmn:sequenceFlow id="Flow_Gateway_Dog" name="dog" sourceRef="Gateway_ClassResult" targetRef="Task_DogStores" />
    <bpmn:sequenceFlow id="Flow_Cat_Merge" sourceRef="Task_CatPlan" targetRef="Gateway_Merge" />
    <bpmn:sequenceFlow id="Flow_Dog_Merge" sourceRef="Task_DogStores" targetRef="Gateway_Merge" />
    <bpmn:sequenceFlow id="Flow_Merge_End" sourceRef="Gateway_Merge" targetRef="EndEvent_1" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_ImageClassify">
    <bpmndi:BPMNPlane id="BPMNPlane_ImageClassify" bpmnElement="Process_ImageClassify">
      <bpmndi:BPMNShape id="StartEvent_1_di" bpmnElement="StartEvent_1">
        <dc:Bounds x="255" y="200" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="261" y="236" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Task_UploadImage_di" bpmnElement="Task_UploadImage">
        <dc:Bounds x="335" y="180" width="120" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Task_ClassifyImage_di" bpmnElement="Task_ClassifyImage">
        <dc:Bounds x="495" y="180" width="150" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_ClassResult_di" bpmnElement="Gateway_ClassResult" isMarkerVisible="true">
        <dc:Bounds x="695" y="195" width="50" height="50" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="690" y="245" width="60" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="EndEvent_1_di" bpmnElement="EndEvent_1">
        <dc:Bounds x="1392" y="200" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1400" y="236" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_Merge_di" bpmnElement="Gateway_Merge" isMarkerVisible="true">
        <dc:Bounds x="1215" y="193" width="50" height="50" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1224" y="243" width="32" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Task_CatPlan_di" bpmnElement="Task_CatPlan">
        <dc:Bounds x="865" y="100" width="170" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Task_DogStores_di" bpmnElement="Task_DogStores">
        <dc:Bounds x="895" y="290" width="190" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_Start_Upload_di" bpmnElement="Flow_Start_Upload">
        <di:waypoint x="291" y="218" />
        <di:waypoint x="335" y="220" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_Upload_Classify_di" bpmnElement="Flow_Upload_Classify">
        <di:waypoint x="455" y="220" />
        <di:waypoint x="495" y="220" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_Classify_Gateway_di" bpmnElement="Flow_Classify_Gateway">
        <di:waypoint x="645" y="220" />
        <di:waypoint x="695" y="220" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_Gateway_Cat_di" bpmnElement="Flow_Gateway_Cat">
        <di:waypoint x="745" y="220" />
        <di:waypoint x="785" y="220" />
        <di:waypoint x="785" y="140" />
        <di:waypoint x="865" y="140" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="793" y="170" width="15" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_Gateway_Dog_di" bpmnElement="Flow_Gateway_Dog">
        <di:waypoint x="745" y="220" />
        <di:waypoint x="785" y="220" />
        <di:waypoint x="785" y="330" />
        <di:waypoint x="895" y="330" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="791" y="265" width="19" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_Cat_Merge_di" bpmnElement="Flow_Cat_Merge">
        <di:waypoint x="1035" y="120" />
        <di:waypoint x="1130" y="120" />
        <di:waypoint x="1130" y="218" />
        <di:waypoint x="1215" y="218" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_Dog_Merge_di" bpmnElement="Flow_Dog_Merge">
        <di:waypoint x="1085" y="330" />
        <di:waypoint x="1130" y="330" />
        <di:waypoint x="1130" y="218" />
        <di:waypoint x="1215" y="218" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_Merge_End_di" bpmnElement="Flow_Merge_End">
        <di:waypoint x="1264" y="219" />
        <di:waypoint x="1329" y="219" />
        <di:waypoint x="1329" y="217" />
        <di:waypoint x="1392" y="217" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
', 0, NULL, 0, NULL, NULL, NULL, '2025-12-04 11:47:03.870942+08', '2025-12-21 15:39:52.996223+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (33, 'd57de828-7c47-4908-b921-e32047fb6967', '1', 'HelloWorldProcess_1658', 'HelloWorldProcess_Code_1658', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="utf-8"?><bpmn:definitions targetNamespace="http://bpmn.io/schema/bpmn" id="bpmn-diagram" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="d57de828-7c47-4908-b921-e32047fb6967" name="HelloWorldProcess_1658" isExecutable="true" sf:code="HelloWorldProcess_Code_1658" sf:version="1"><bpmn:startEvent id="StartNode_2739" name="Start" sf:code="Start"><bpmn:outgoing>Flow_6514</bpmn:outgoing></bpmn:startEvent><bpmn:task id="TaskNode_6721" name="Hello" sf:code="003"><bpmn:incoming>Flow_6514</bpmn:incoming><bpmn:outgoing>Flow_5891</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_6901" name="World" sf:code="005"><bpmn:incoming>Flow_5891</bpmn:incoming><bpmn:outgoing>Flow_3317</bpmn:outgoing></bpmn:task><bpmn:endEvent id="EndNode_1369" name="End" sf:code="End"><bpmn:incoming>Flow_3317</bpmn:incoming></bpmn:endEvent><bpmn:sequenceFlow id="Flow_6514" name="" sourceRef="StartNode_2739" targetRef="TaskNode_6721" /><bpmn:sequenceFlow id="Flow_5891" name="" sourceRef="TaskNode_6721" targetRef="TaskNode_6901" /><bpmn:sequenceFlow id="Flow_3317" name="" sourceRef="TaskNode_6901" targetRef="EndNode_1369" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100"><bpmndi:BPMNShape id="BPMNShape_7jbei3l_di" bpmnElement="StartNode_2739"><dc:Bounds height="36" width="36" x="240" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_kwniyyg_di" bpmnElement="TaskNode_6721"><dc:Bounds height="80" width="100" x="356" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_lcqlrwg_di" bpmnElement="TaskNode_6901"><dc:Bounds height="80" width="100" x="536" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_e7n25ue_di" bpmnElement="EndNode_1369"><dc:Bounds height="36" width="36" x="716" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Flow_6514_di" bpmnElement="Flow_6514"><di:waypoint x="276" y="198" /><di:waypoint x="356" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_5891_di" bpmnElement="Flow_5891"><di:waypoint x="456" y="198" /><di:waypoint x="536" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_3317_di" bpmnElement="Flow_3317"><di:waypoint x="636" y="198" /><di:waypoint x="716" y="198" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, 0, NULL, NULL, NULL, '2025-12-04 13:25:02.984378+08', NULL, NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (35, 'Process_BookRenewal_sme1', '1', '图书续借流程', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="Process_BookRenewal_sme1" name="图书续借流程" isExecutable="false"><bpmn:startEvent id="AStartEvent" name="开始续借" /><bpmn:serviceTask id="ACheckDueDate" name="检查还书日期" /><bpmn:exclusiveGateway id="AIsWithinRenewalPeriod" name="是否在可续借期限内" /><bpmn:serviceTask id="ACheckRenewalCount" name="检查续借次数" /><bpmn:exclusiveGateway id="AHasReachedMaxRenewals" name="已达到最大续借次数" /><bpmn:userTask id="ARenewSuccess" name="续借成功" /><bpmn:userTask id="ARenewFailure" name="续借失败" /><bpmn:endEvent id="AEndEvent_Success" name="结束（成功）" /><bpmn:endEvent id="AEndEvent_Failure" name="结束（失败）" /><bpmn:sequenceFlow id="Flow_AStartEvent_ACheckDueDate" sourceRef="AStartEvent" targetRef="ACheckDueDate" /><bpmn:sequenceFlow id="Flow_ACheckDueDate_AIsWithinRenewalPeriod" sourceRef="ACheckDueDate" targetRef="AIsWithinRenewalPeriod" /><bpmn:sequenceFlow id="Flow_AIsWithinRenewalPeriod_ACheckRenewalCount" sourceRef="AIsWithinRenewalPeriod" targetRef="ACheckRenewalCount" /><bpmn:sequenceFlow id="Flow_AIsWithinRenewalPeriod_ARenewFailure" sourceRef="AIsWithinRenewalPeriod" targetRef="ARenewFailure" /><bpmn:sequenceFlow id="Flow_ACheckRenewalCount_AHasReachedMaxRenewals" sourceRef="ACheckRenewalCount" targetRef="AHasReachedMaxRenewals" /><bpmn:sequenceFlow id="Flow_AHasReachedMaxRenewals_ARenewSuccess" sourceRef="AHasReachedMaxRenewals" targetRef="ARenewSuccess" /><bpmn:sequenceFlow id="Flow_AHasReachedMaxRenewals_ARenewFailure" sourceRef="AHasReachedMaxRenewals" targetRef="ARenewFailure" /><bpmn:sequenceFlow id="Flow_ARenewSuccess_AEndEvent_Success" sourceRef="ARenewSuccess" targetRef="AEndEvent_Success" /><bpmn:sequenceFlow id="Flow_ARenewFailure_AEndEvent_Failure" sourceRef="ARenewFailure" targetRef="AEndEvent_Failure" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_BookRenewal_sme1"><bpmndi:BPMNShape id="Shape_AStartEvent" bpmnElement="AStartEvent"><dc:Bounds x="150" y="162" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ACheckDueDate" bpmnElement="ACheckDueDate"><dc:Bounds x="350" y="140" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AIsWithinRenewalPeriod" bpmnElement="AIsWithinRenewalPeriod"><dc:Bounds x="550" y="162" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ACheckRenewalCount" bpmnElement="ACheckRenewalCount"><dc:Bounds x="750" y="261" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AHasReachedMaxRenewals" bpmnElement="AHasReachedMaxRenewals"><dc:Bounds x="950" y="283" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ARenewSuccess" bpmnElement="ARenewSuccess"><dc:Bounds x="1150" y="301" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ARenewFailure" bpmnElement="ARenewFailure"><dc:Bounds x="1150" y="140" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent_Success" bpmnElement="AEndEvent_Success"><dc:Bounds x="1350" y="323" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent_Failure" bpmnElement="AEndEvent_Failure"><dc:Bounds x="1350" y="162" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AStartEvent_ACheckDueDate" bpmnElement="Flow_AStartEvent_ACheckDueDate"><di:waypoint x="186" y="180" /><di:waypoint x="236" y="180" /><di:waypoint x="236" y="180" /><di:waypoint x="350" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ACheckDueDate_AIsWithinRenewalPeriod" bpmnElement="Flow_ACheckDueDate_AIsWithinRenewalPeriod"><di:waypoint x="450" y="180" /><di:waypoint x="500" y="180" /><di:waypoint x="500" y="180" /><di:waypoint x="550" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AIsWithinRenewalPeriod_ACheckRenewalCount" bpmnElement="Flow_AIsWithinRenewalPeriod_ACheckRenewalCount"><di:waypoint x="586" y="180" /><di:waypoint x="636" y="180" /><di:waypoint x="636" y="301" /><di:waypoint x="750" y="301" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AIsWithinRenewalPeriod_ARenewFailure" bpmnElement="Flow_AIsWithinRenewalPeriod_ARenewFailure"><di:waypoint x="586" y="180" /><di:waypoint x="636" y="180" /><di:waypoint x="636" y="180" /><di:waypoint x="1150" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ACheckRenewalCount_AHasReachedMaxRenewals" bpmnElement="Flow_ACheckRenewalCount_AHasReachedMaxRenewals"><di:waypoint x="850" y="301" /><di:waypoint x="900" y="301" /><di:waypoint x="900" y="301" /><di:waypoint x="950" y="301" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AHasReachedMaxRenewals_ARenewSuccess" bpmnElement="Flow_AHasReachedMaxRenewals_ARenewSuccess"><di:waypoint x="986" y="301" /><di:waypoint x="1036" y="301" /><di:waypoint x="1036" y="341" /><di:waypoint x="1150" y="341" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AHasReachedMaxRenewals_ARenewFailure" bpmnElement="Flow_AHasReachedMaxRenewals_ARenewFailure"><di:waypoint x="986" y="301" /><di:waypoint x="1036" y="301" /><di:waypoint x="1036" y="180" /><di:waypoint x="1150" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ARenewSuccess_AEndEvent_Success" bpmnElement="Flow_ARenewSuccess_AEndEvent_Success"><di:waypoint x="1250" y="341" /><di:waypoint x="1300" y="341" /><di:waypoint x="1300" y="341" /><di:waypoint x="1350" y="341" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ARenewFailure_AEndEvent_Failure" bpmnElement="Flow_ARenewFailure_AEndEvent_Failure"><di:waypoint x="1250" y="180" /><di:waypoint x="1300" y="180" /><di:waypoint x="1300" y="180" /><di:waypoint x="1350" y="180" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, 0, NULL, NULL, NULL, '2025-12-07 12:09:01.736971+08', NULL, NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (40, 'Process_2424', '1', 'Process_Variable_Test_2026', 'Process_Code_2424', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_2424" sf:code="Process_Code_2424" name="Process_Variable_Test_2026" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="StartEvent_1" name="Start">
      <bpmn:outgoing>Flow_181eoxw</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:task id="Activity_1x1t74f" name="test">
      <bpmn:extensionElements>
        <sf:sections>
          <sf:section name="myProperties">{"id": "110"}</sf:section>
        </sf:sections>
        <sf:notifications>
          <sf:notification name="Fisher" outerId="11" outerCode="" outerType="User" />
        </sf:notifications>
        <sf:variables>
          <sf:variable name="ImageContent" type="String" defaultValue="" direction="output" isReferenced="false" required="true" />
        </sf:variables>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_181eoxw</bpmn:incoming>
      <bpmn:outgoing>Flow_1gruj39</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_181eoxw" sourceRef="StartEvent_1" targetRef="Activity_1x1t74f" />
    <bpmn:task id="Activity_0smnlah" name="review">
      <bpmn:extensionElements>
        <sf:variables>
          <sf:variable name="ImangeContent" type="String" defaultValue="" direction="input" isReferenced="true" required="true">
            <sf:varRefDetail sourceRef="Activity_1x1t74f" variableName="ImageContent" />
          </sf:variable>
          <sf:variable name="result" type="String" defaultValue="cat/dog" direction="output" isReferenced="false" required="true" />
        </sf:variables>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_1gruj39</bpmn:incoming>
      <bpmn:outgoing>Flow_0tbt6ck</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_1gruj39" sourceRef="Activity_1x1t74f" targetRef="Activity_0smnlah" />
    <bpmn:endEvent id="Event_1jg0pye" name="End">
      <bpmn:incoming>Flow_0tbt6ck</bpmn:incoming>
    </bpmn:endEvent>
    <bpmn:sequenceFlow id="Flow_0tbt6ck" sourceRef="Activity_0smnlah" targetRef="Event_1jg0pye" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_2424">
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1x1t74f_di" bpmnElement="Activity_1x1t74f">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0smnlah_di" bpmnElement="Activity_0smnlah">
        <dc:Bounds x="660" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1jg0pye_di" bpmnElement="Event_1jg0pye">
        <dc:Bounds x="822" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="830" y="283" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_181eoxw_di" bpmnElement="Flow_181eoxw">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1gruj39_di" bpmnElement="Flow_1gruj39">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="660" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0tbt6ck_di" bpmnElement="Flow_0tbt6ck">
        <di:waypoint x="760" y="258" />
        <di:waypoint x="822" y="258" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
                      ', 0, NULL, 0, NULL, NULL, NULL, '2025-12-10 08:43:51.976286+08', '2025-12-13 09:27:06.091063+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (50, 'Process_BeeHoneyCollection_yop1', '1', '蜜蜂采蜜流程', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="Process_BeeHoneyCollection_yop1" name="蜜蜂采蜜流程" isExecutable="false"><bpmn:startEvent id="AStartEvent" name="开始采蜜任务" /><bpmn:userTask id="AActivity_ScoutForFlowers" name="侦查员蜜蜂寻找花源" /><bpmn:exclusiveGateway id="AExclusiveGateway_FoundFlowers" name="是否发现充足花源" /><bpmn:userTask id="AActivity_ReturnToHive" name="返回蜂巢报告位置" /><bpmn:serviceTask id="AActivity_DanceCommunication" name="跳摇摆舞传递信息" /><bpmn:userTask id="AActivity_ForagerBeesCollect" name="采集蜂前往采集花蜜" /><bpmn:userTask id="AActivity_TransportNectar" name="运输花蜜回蜂巢" /><bpmn:serviceTask id="AActivity_ProcessNectar" name="酿造蜂蜜并储存" /><bpmn:endEvent id="AEndEvent_Success" name="完成采蜜任务" /><bpmn:endEvent id="AEndEvent_NoFlowers" name="未发现花源，任务结束" /><bpmn:sequenceFlow id="Flow_AStartEvent_AActivity_ScoutForFlowers" sourceRef="AStartEvent" targetRef="AActivity_ScoutForFlowers" /><bpmn:sequenceFlow id="Flow_AActivity_ScoutForFlowers_AExclusiveGateway_FoundFlowers" sourceRef="AActivity_ScoutForFlowers" targetRef="AExclusiveGateway_FoundFlowers" /><bpmn:sequenceFlow id="Flow_AExclusiveGateway_FoundFlowers_AActivity_ReturnToHive" sourceRef="AExclusiveGateway_FoundFlowers" targetRef="AActivity_ReturnToHive"><bpmn:conditionExpression>true</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_AExclusiveGateway_FoundFlowers_AEndEvent_NoFlowers" sourceRef="AExclusiveGateway_FoundFlowers" targetRef="AEndEvent_NoFlowers"><bpmn:conditionExpression>false</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_AActivity_ReturnToHive_AActivity_DanceCommunication" sourceRef="AActivity_ReturnToHive" targetRef="AActivity_DanceCommunication" /><bpmn:sequenceFlow id="Flow_AActivity_DanceCommunication_AActivity_ForagerBeesCollect" sourceRef="AActivity_DanceCommunication" targetRef="AActivity_ForagerBeesCollect" /><bpmn:sequenceFlow id="Flow_AActivity_ForagerBeesCollect_AActivity_TransportNectar" sourceRef="AActivity_ForagerBeesCollect" targetRef="AActivity_TransportNectar" /><bpmn:sequenceFlow id="Flow_AActivity_TransportNectar_AActivity_ProcessNectar" sourceRef="AActivity_TransportNectar" targetRef="AActivity_ProcessNectar" /><bpmn:sequenceFlow id="Flow_AActivity_ProcessNectar_AEndEvent_Success" sourceRef="AActivity_ProcessNectar" targetRef="AEndEvent_Success" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_BeeHoneyCollection_yop1"><bpmndi:BPMNShape id="Shape_AStartEvent" bpmnElement="AStartEvent"><dc:Bounds x="150" y="162" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_ScoutForFlowers" bpmnElement="AActivity_ScoutForFlowers"><dc:Bounds x="350" y="140" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AExclusiveGateway_FoundFlowers" bpmnElement="AExclusiveGateway_FoundFlowers"><dc:Bounds x="550" y="162" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_ReturnToHive" bpmnElement="AActivity_ReturnToHive"><dc:Bounds x="750" y="301" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_DanceCommunication" bpmnElement="AActivity_DanceCommunication"><dc:Bounds x="950" y="301" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_ForagerBeesCollect" bpmnElement="AActivity_ForagerBeesCollect"><dc:Bounds x="1150" y="301" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_TransportNectar" bpmnElement="AActivity_TransportNectar"><dc:Bounds x="1350" y="301" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AActivity_ProcessNectar" bpmnElement="AActivity_ProcessNectar"><dc:Bounds x="1550" y="301" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent_Success" bpmnElement="AEndEvent_Success"><dc:Bounds x="1750" y="300" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent_NoFlowers" bpmnElement="AEndEvent_NoFlowers"><dc:Bounds x="750" y="140" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AStartEvent_AActivity_ScoutForFlowers" bpmnElement="Flow_AStartEvent_AActivity_ScoutForFlowers"><di:waypoint x="186" y="180" /><di:waypoint x="236" y="180" /><di:waypoint x="236" y="180" /><di:waypoint x="350" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_ScoutForFlowers_AExclusiveGateway_FoundFlowers" bpmnElement="Flow_AActivity_ScoutForFlowers_AExclusiveGateway_FoundFlowers"><di:waypoint x="450" y="180" /><di:waypoint x="500" y="180" /><di:waypoint x="500" y="180" /><di:waypoint x="550" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AExclusiveGateway_FoundFlowers_AActivity_ReturnToHive" bpmnElement="Flow_AExclusiveGateway_FoundFlowers_AActivity_ReturnToHive"><di:waypoint x="586" y="180" /><di:waypoint x="636" y="180" /><di:waypoint x="636" y="341" /><di:waypoint x="750" y="341" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AExclusiveGateway_FoundFlowers_AEndEvent_NoFlowers" bpmnElement="Flow_AExclusiveGateway_FoundFlowers_AEndEvent_NoFlowers"><di:waypoint x="586" y="180" /><di:waypoint x="636" y="180" /><di:waypoint x="636" y="158" /><di:waypoint x="750" y="158" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_ReturnToHive_AActivity_DanceCommunication" bpmnElement="Flow_AActivity_ReturnToHive_AActivity_DanceCommunication"><di:waypoint x="850" y="341" /><di:waypoint x="900" y="341" /><di:waypoint x="950" y="341" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_DanceCommunication_AActivity_ForagerBeesCollect" bpmnElement="Flow_AActivity_DanceCommunication_AActivity_ForagerBeesCollect"><di:waypoint x="1050" y="341" /><di:waypoint x="1100" y="341" /><di:waypoint x="1150" y="341" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_ForagerBeesCollect_AActivity_TransportNectar" bpmnElement="Flow_AActivity_ForagerBeesCollect_AActivity_TransportNectar"><di:waypoint x="1250" y="341" /><di:waypoint x="1300" y="341" /><di:waypoint x="1350" y="341" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_TransportNectar_AActivity_ProcessNectar" bpmnElement="Flow_AActivity_TransportNectar_AActivity_ProcessNectar"><di:waypoint x="1450" y="341" /><di:waypoint x="1500" y="341" /><di:waypoint x="1550" y="341" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AActivity_ProcessNectar_AEndEvent_Success" bpmnElement="Flow_AActivity_ProcessNectar_AEndEvent_Success"><di:waypoint x="1650" y="341" /><di:waypoint x="1700" y="341" /><di:waypoint x="1700" y="318" /><di:waypoint x="1750" y="318" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, 0, NULL, NULL, NULL, '2025-12-13 02:34:44.158932+08', NULL, NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (58, 'Process_9689', '1', 'Process_InterEvent_009', 'Process_Code_9689', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_9689" sf:code="Process_Code_9689" name="Process_InterEvent_009" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="StartEvent_1" name="Start">
      <bpmn:outgoing>Flow_013o9rw</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:task id="Activity_1ku9dr5" name="submit">
      <bpmn:incoming>Flow_013o9rw</bpmn:incoming>
      <bpmn:outgoing>Flow_0kj0rf4</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_013o9rw" sourceRef="StartEvent_1" targetRef="Activity_1ku9dr5" />
    <bpmn:intermediateThrowEvent id="Event_1c34ryo" name="milestone">
      <bpmn:incoming>Flow_0kj0rf4</bpmn:incoming>
      <bpmn:outgoing>Flow_02dojgx</bpmn:outgoing>
    </bpmn:intermediateThrowEvent>
    <bpmn:sequenceFlow id="Flow_0kj0rf4" sourceRef="Activity_1ku9dr5" targetRef="Event_1c34ryo" />
    <bpmn:task id="Activity_1a16wni" name="Dept Manager Approval">
      <bpmn:incoming>Flow_02dojgx</bpmn:incoming>
      <bpmn:outgoing>Flow_1kqe471</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_02dojgx" sourceRef="Event_1c34ryo" targetRef="Activity_1a16wni" />
    <bpmn:endEvent id="Event_0ikxssq" name="End">
      <bpmn:incoming>Flow_1kqe471</bpmn:incoming>
    </bpmn:endEvent>
    <bpmn:sequenceFlow id="Flow_1kqe471" sourceRef="Activity_1a16wni" targetRef="Event_0ikxssq" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_9689">
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1c34ryo_di" bpmnElement="Event_1c34ryo">
        <dc:Bounds x="762" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="758" y="283" width="48" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1ku9dr5_di" bpmnElement="Activity_1ku9dr5">
        <dc:Bounds x="560" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1a16wni_di" bpmnElement="Activity_1a16wni">
        <dc:Bounds x="900" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0ikxssq_di" bpmnElement="Event_0ikxssq">
        <dc:Bounds x="1102" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1110" y="283" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_013o9rw_di" bpmnElement="Flow_013o9rw">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="560" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0kj0rf4_di" bpmnElement="Flow_0kj0rf4">
        <di:waypoint x="660" y="258" />
        <di:waypoint x="762" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_02dojgx_di" bpmnElement="Flow_02dojgx">
        <di:waypoint x="798" y="258" />
        <di:waypoint x="900" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1kqe471_di" bpmnElement="Flow_1kqe471">
        <di:waypoint x="1000" y="258" />
        <di:waypoint x="1102" y="258" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
                                                                                                                                                                                                                                       ', 0, NULL, 0, NULL, NULL, NULL, '2025-12-18 22:53:38.002275+08', '2025-12-18 14:54:52.908408+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (59, 'Process_6018', '1', 'Process_ImageClassificaton_Simple', 'Process_Code_6018', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_6018" sf:code="Process_Code_6018" name="Process_ImageClassificaton_Simple" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="StartEvent_1" name="Start">
      <bpmn:outgoing>Flow_15qmiqn</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:sequenceFlow id="Flow_15qmiqn" sourceRef="StartEvent_1" targetRef="Activity_0gl77ou" />
    <bpmn:serviceTask id="Activity_073qf5r" name="Image Classification">
      <bpmn:extensionElements>
        <sf:aIServices>
          <sf:aIService configUUID="37f634fc-6c72-4a95-bf8c-5953b04be4cb" type="LLM" />
        </sf:aIServices>
        <sf:variables>
          <sf:variable name="ImageContent" type="String" defaultValue="" direction="Input" isReferenced="true" isRequired="true">
            <sf:varRefDetail sourceRef="Activity_0gl77ou" variableName="ImageContent" />
          </sf:variable>
          <sf:variable name="result" type="String" defaultValue="" direction="Output" isReferenced="false" isRequired="true" />
        </sf:variables>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_1frfau4</bpmn:incoming>
      <bpmn:outgoing>Flow_1cgi5mr</bpmn:outgoing>
    </bpmn:serviceTask>
    <bpmn:sequenceFlow id="Flow_1frfau4" sourceRef="Activity_0gl77ou" targetRef="Activity_073qf5r" />
    <bpmn:task id="Activity_0cmw4ki" name="Reivew">
      <bpmn:incoming>Flow_1cgi5mr</bpmn:incoming>
      <bpmn:outgoing>Flow_0rnghj0</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_1cgi5mr" sourceRef="Activity_073qf5r" targetRef="Activity_0cmw4ki" />
    <bpmn:endEvent id="Event_1rqu53e" name="End">
      <bpmn:incoming>Flow_0rnghj0</bpmn:incoming>
    </bpmn:endEvent>
    <bpmn:sequenceFlow id="Flow_0rnghj0" sourceRef="Activity_0cmw4ki" targetRef="Event_1rqu53e" />
    <bpmn:userTask id="Activity_0gl77ou" name="Upload Image">
      <bpmn:extensionElements>
        <sf:variables>
          <sf:variable name="ImageContent" type="String" defaultValue="" direction="Output" isReferenced="false" isRequired="true" />
        </sf:variables>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_15qmiqn</bpmn:incoming>
      <bpmn:outgoing>Flow_1frfau4</bpmn:outgoing>
    </bpmn:userTask>
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_6018">
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_073qf5r_di" bpmnElement="Activity_073qf5r">
        <dc:Bounds x="700" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0cmw4ki_di" bpmnElement="Activity_0cmw4ki">
        <dc:Bounds x="900" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1rqu53e_di" bpmnElement="Event_1rqu53e">
        <dc:Bounds x="1102" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1110" y="283" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0jbatyx_di" bpmnElement="Activity_0gl77ou">
        <dc:Bounds x="500" y="218" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_15qmiqn_di" bpmnElement="Flow_15qmiqn">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1frfau4_di" bpmnElement="Flow_1frfau4">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="700" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1cgi5mr_di" bpmnElement="Flow_1cgi5mr">
        <di:waypoint x="800" y="258" />
        <di:waypoint x="900" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0rnghj0_di" bpmnElement="Flow_0rnghj0">
        <di:waypoint x="1000" y="258" />
        <di:waypoint x="1102" y="258" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
', 0, NULL, 0, NULL, NULL, NULL, '2025-12-19 08:23:30.462976+08', '2025-12-18 16:24:40.248831+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (60, 'Process_AircraftTakeoff_lsma', '1', 'Aircraft Takeoff Process', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL">
  <bpmn:process id="Process_AircraftTakeoff_lsma" name="Aircraft Takeoff Process" isExecutable="false">
    <bpmn:startEvent id="AStartEvent" name="Flight Initiation" />
    <bpmn:userTask id="AActivity_PreFlightCheck" name="Pre-Flight Check" />
    <bpmn:serviceTask id="AActivity_FuelVerification" name="Fuel and Weight Verification" />
    <bpmn:exclusiveGateway id="AGateway_WeatherCheck" name="Weather Conditions Acceptable?" />
    <bpmn:userTask id="AActivity_TaxiToRunway" name="Taxi to Runway" />
    <bpmn:userTask id="AActivity_FinalClearance" name="Obtain Final Clearance" />
    <bpmn:exclusiveGateway id="AGateway_ClearanceApproved" name="Clearance Approved?" />
    <bpmn:userTask id="AActivity_Takeoff" name="Takeoff" />
    <bpmn:endEvent id="AEndEvent_Success" name="Successful Takeoff" />
    <bpmn:endEvent id="AEndEvent_Delay" name="Delay Due to Weather" />
    <bpmn:endEvent id="AEndEvent_Canceled" name="Takeoff Canceled" />
    <bpmn:sequenceFlow id="Flow_AStartEvent_AActivity_PreFlightCheck" sourceRef="AStartEvent" targetRef="AActivity_PreFlightCheck" />
    <bpmn:sequenceFlow id="Flow_AActivity_PreFlightCheck_AActivity_FuelVerification" sourceRef="AActivity_PreFlightCheck" targetRef="AActivity_FuelVerification" />
    <bpmn:sequenceFlow id="Flow_AActivity_FuelVerification_AGateway_WeatherCheck" sourceRef="AActivity_FuelVerification" targetRef="AGateway_WeatherCheck" />
    <bpmn:sequenceFlow id="Flow_AGateway_WeatherCheck_AActivity_TaxiToRunway" sourceRef="AGateway_WeatherCheck" targetRef="AActivity_TaxiToRunway" />
    <bpmn:sequenceFlow id="Flow_AGateway_WeatherCheck_AEndEvent_Delay" sourceRef="AGateway_WeatherCheck" targetRef="AEndEvent_Delay" />
    <bpmn:sequenceFlow id="Flow_AActivity_TaxiToRunway_AActivity_FinalClearance" sourceRef="AActivity_TaxiToRunway" targetRef="AActivity_FinalClearance" />
    <bpmn:sequenceFlow id="Flow_AActivity_FinalClearance_AGateway_ClearanceApproved" sourceRef="AActivity_FinalClearance" targetRef="AGateway_ClearanceApproved" />
    <bpmn:sequenceFlow id="Flow_AGateway_ClearanceApproved_AActivity_Takeoff" sourceRef="AGateway_ClearanceApproved" targetRef="AActivity_Takeoff" />
    <bpmn:sequenceFlow id="Flow_AGateway_ClearanceApproved_AEndEvent_Canceled" sourceRef="AGateway_ClearanceApproved" targetRef="AEndEvent_Canceled" />
    <bpmn:sequenceFlow id="Flow_AActivity_Takeoff_AEndEvent_Success" sourceRef="AActivity_Takeoff" targetRef="AEndEvent_Success" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_AircraftTakeoff_lsma">
      <bpmndi:BPMNShape id="Shape_AStartEvent" bpmnElement="AStartEvent">
        <dc:Bounds x="150" y="162" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AActivity_PreFlightCheck" bpmnElement="AActivity_PreFlightCheck">
        <dc:Bounds x="350" y="140" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AActivity_FuelVerification" bpmnElement="AActivity_FuelVerification">
        <dc:Bounds x="550" y="140" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AGateway_WeatherCheck" bpmnElement="AGateway_WeatherCheck" isMarkerVisible="true">
        <dc:Bounds x="750" y="162" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AActivity_TaxiToRunway" bpmnElement="AActivity_TaxiToRunway">
        <dc:Bounds x="950" y="301" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AActivity_FinalClearance" bpmnElement="AActivity_FinalClearance">
        <dc:Bounds x="1150" y="301" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AGateway_ClearanceApproved" bpmnElement="AGateway_ClearanceApproved" isMarkerVisible="true">
        <dc:Bounds x="1350" y="323" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AActivity_Takeoff" bpmnElement="AActivity_Takeoff">
        <dc:Bounds x="1550" y="462" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AEndEvent_Success" bpmnElement="AEndEvent_Success">
        <dc:Bounds x="1750" y="484" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AEndEvent_Delay" bpmnElement="AEndEvent_Delay">
        <dc:Bounds x="950" y="140" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AEndEvent_Canceled" bpmnElement="AEndEvent_Canceled">
        <dc:Bounds x="1550" y="301" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Edge_AStartEvent_AActivity_PreFlightCheck" bpmnElement="Flow_AStartEvent_AActivity_PreFlightCheck">
        <di:waypoint x="186" y="180" />
        <di:waypoint x="236" y="180" />
        <di:waypoint x="236" y="180" />
        <di:waypoint x="350" y="180" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AActivity_PreFlightCheck_AActivity_FuelVerification" bpmnElement="Flow_AActivity_PreFlightCheck_AActivity_FuelVerification">
        <di:waypoint x="450" y="180" />
        <di:waypoint x="500" y="180" />
        <di:waypoint x="550" y="180" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AActivity_FuelVerification_AGateway_WeatherCheck" bpmnElement="Flow_AActivity_FuelVerification_AGateway_WeatherCheck">
        <di:waypoint x="650" y="180" />
        <di:waypoint x="700" y="180" />
        <di:waypoint x="700" y="180" />
        <di:waypoint x="750" y="180" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AGateway_WeatherCheck_AActivity_TaxiToRunway" bpmnElement="Flow_AGateway_WeatherCheck_AActivity_TaxiToRunway">
        <di:waypoint x="786" y="180" />
        <di:waypoint x="836" y="180" />
        <di:waypoint x="836" y="341" />
        <di:waypoint x="950" y="341" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AGateway_WeatherCheck_AEndEvent_Delay" bpmnElement="Flow_AGateway_WeatherCheck_AEndEvent_Delay">
        <di:waypoint x="786" y="180" />
        <di:waypoint x="836" y="180" />
        <di:waypoint x="836" y="158" />
        <di:waypoint x="950" y="158" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AActivity_TaxiToRunway_AActivity_FinalClearance" bpmnElement="Flow_AActivity_TaxiToRunway_AActivity_FinalClearance">
        <di:waypoint x="1050" y="341" />
        <di:waypoint x="1100" y="341" />
        <di:waypoint x="1150" y="341" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AActivity_FinalClearance_AGateway_ClearanceApproved" bpmnElement="Flow_AActivity_FinalClearance_AGateway_ClearanceApproved">
        <di:waypoint x="1250" y="341" />
        <di:waypoint x="1300" y="341" />
        <di:waypoint x="1300" y="341" />
        <di:waypoint x="1350" y="341" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AGateway_ClearanceApproved_AActivity_Takeoff" bpmnElement="Flow_AGateway_ClearanceApproved_AActivity_Takeoff">
        <di:waypoint x="1386" y="341" />
        <di:waypoint x="1436" y="341" />
        <di:waypoint x="1436" y="502" />
        <di:waypoint x="1550" y="502" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AGateway_ClearanceApproved_AEndEvent_Canceled" bpmnElement="Flow_AGateway_ClearanceApproved_AEndEvent_Canceled">
        <di:waypoint x="1386" y="341" />
        <di:waypoint x="1436" y="341" />
        <di:waypoint x="1436" y="319" />
        <di:waypoint x="1550" y="319" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AActivity_Takeoff_AEndEvent_Success" bpmnElement="Flow_AActivity_Takeoff_AEndEvent_Success">
        <di:waypoint x="1650" y="502" />
        <di:waypoint x="1700" y="502" />
        <di:waypoint x="1700" y="502" />
        <di:waypoint x="1750" y="502" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
', 0, NULL, 0, NULL, NULL, NULL, '2025-12-19 01:42:02.86906+08', '2025-12-18 17:42:09.874224+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (95, 'Process_8733', '1', 'Process_Name_8733', 'Process_Code_8733', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_8733" sf:code="Process_Code_8733" name="Process_Name_8733" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="StartEvent_1" name="Start">
      <bpmn:outgoing>Flow_0ul66k4</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:task id="Activity_0yqfrxi" name="Upload Document">
      <bpmn:incoming>Flow_0ul66k4</bpmn:incoming>
      <bpmn:outgoing>Flow_1r4orak</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_0ul66k4" sourceRef="StartEvent_1" targetRef="Activity_0yqfrxi" />
    <bpmn:serviceTask id="Activity_16wjn7g" name="RAG">
      <bpmn:extensionElements>
        <sf:aiServices>
          <sf:aiService configUUID="ee7781e1-0959-40f8-c673-b6e6d3346748" type="RAG" />
        </sf:aiServices>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_1r4orak</bpmn:incoming>
    </bpmn:serviceTask>
    <bpmn:sequenceFlow id="Flow_1r4orak" sourceRef="Activity_0yqfrxi" targetRef="Activity_16wjn7g" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_8733">
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0yqfrxi_di" bpmnElement="Activity_0yqfrxi">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_16wjn7g_di" bpmnElement="Activity_16wjn7g">
        <dc:Bounds x="710" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_0ul66k4_di" bpmnElement="Flow_0ul66k4">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1r4orak_di" bpmnElement="Flow_1r4orak">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="710" y="258" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                ', 0, NULL, 0, NULL, NULL, NULL, '2025-12-21 18:19:27.290479+08', '2025-12-21 18:19:27.290479+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (97, 'Process_6665', '1', 'Process_ImageClassification_simple', 'Process_Code_6665', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_6665" sf:code="Process_Code_6665" name="Process_ImageClassification_simple" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="StartEvent_1" name="Start">
      <bpmn:outgoing>Flow_1rcwv95</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:serviceTask id="Activity_16jels2" name="Image Classification">
      <bpmn:extensionElements>
        <sf:aiServices>
          <sf:aiService configUUID="20d6b351-6204-41e4-c8f0-366333fe5e56" type="LLM" />
        </sf:aiServices>
        <sf:variables>
          <sf:variable name="ImageContent" type="Object" defaultValue="" direction="Input" isReferenced="true" isRequired="true">
            <sf:varRefDetail sourceRef="Activity_0jsj4or" variableName="ImageContent" />
          </sf:variable>
          <sf:variable name="pet" type="String" defaultValue="" direction="Output" isReferenced="false" isRequired="true" />
        </sf:variables>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_1nwkjy1</bpmn:incoming>
      <bpmn:outgoing>Flow_1kmy1gp</bpmn:outgoing>
    </bpmn:serviceTask>
    <bpmn:task id="Activity_0dwnv1a" name="Review Result">
      <bpmn:incoming>Flow_1kmy1gp</bpmn:incoming>
      <bpmn:outgoing>Flow_1qwwid6</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_1kmy1gp" sourceRef="Activity_16jels2" targetRef="Activity_0dwnv1a" />
    <bpmn:endEvent id="Event_1hzocgp" name="End">
      <bpmn:incoming>Flow_1qwwid6</bpmn:incoming>
    </bpmn:endEvent>
    <bpmn:sequenceFlow id="Flow_1qwwid6" sourceRef="Activity_0dwnv1a" targetRef="Event_1hzocgp" />
    <bpmn:task id="Activity_0jsj4or" name="Uplaoad Image">
      <bpmn:extensionElements>
        <sf:variables>
          <sf:variable name="ImageContent" type="Object" defaultValue="" direction="Output" isReferenced="false" isRequired="true" />
        </sf:variables>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_1rcwv95</bpmn:incoming>
      <bpmn:outgoing>Flow_1nwkjy1</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_1rcwv95" sourceRef="StartEvent_1" targetRef="Activity_0jsj4or" />
    <bpmn:sequenceFlow id="Flow_1nwkjy1" sourceRef="Activity_0jsj4or" targetRef="Activity_16jels2" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_6665">
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="262" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="268" y="276" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_16jels2_di" bpmnElement="Activity_16jels2">
        <dc:Bounds x="560" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0dwnv1a_di" bpmnElement="Activity_0dwnv1a">
        <dc:Bounds x="780" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1hzocgp_di" bpmnElement="Event_1hzocgp">
        <dc:Bounds x="1002" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1010" y="283" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0jsj4or_di" bpmnElement="Activity_0jsj4or">
        <dc:Bounds x="370" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_1kmy1gp_di" bpmnElement="Flow_1kmy1gp">
        <di:waypoint x="660" y="258" />
        <di:waypoint x="780" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1qwwid6_di" bpmnElement="Flow_1qwwid6">
        <di:waypoint x="880" y="258" />
        <di:waypoint x="1002" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1rcwv95_di" bpmnElement="Flow_1rcwv95">
        <di:waypoint x="298" y="258" />
        <di:waypoint x="370" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1nwkjy1_di" bpmnElement="Flow_1nwkjy1">
        <di:waypoint x="470" y="258" />
        <di:waypoint x="560" y="258" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
', 0, NULL, 0, NULL, NULL, NULL, '2025-12-24 18:51:49.907062+08', '2025-12-21 19:43:27.762885+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (98, 'Process_7852', '1', 'Process_RAG_7852', 'Process_Code_7852', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_7852" sf:code="Process_Code_7852" name="Process_RAG_7852" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="StartEvent_1" name="Start">
      <bpmn:outgoing>Flow_1hjoyg4</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:task id="Activity_0cj0ef2" name="填写电动汽车零配件需求信息">
      <bpmn:extensionElements>
        <sf:variables>
          <sf:variable name="Requirement" type="String" defaultValue="" direction="Output" isReferenced="false" isRequired="true" />
        </sf:variables>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_1hjoyg4</bpmn:incoming>
      <bpmn:outgoing>Flow_0csdaen</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_1hjoyg4" sourceRef="StartEvent_1" targetRef="Activity_0cj0ef2" />
    <bpmn:serviceTask id="Activity_0ttoxpx" name="RAG智能问答助手">
      <bpmn:extensionElements>
        <sf:aiServices>
          <sf:aiService configUUID="07dc92ff-a191-461a-c22d-2c7cb984c6ab" type="RAG" />
        </sf:aiServices>
        <sf:variables>
          <sf:variable name="Requirement" type="String" defaultValue="" direction="Input" isReferenced="true" isRequired="true">
            <sf:varRefDetail sourceRef="Activity_0cj0ef2" variableName="Requirement" />
          </sf:variable>
          <sf:variable name="Answer" type="String" defaultValue="" direction="Output" isReferenced="false" isRequired="true" />
        </sf:variables>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_0csdaen</bpmn:incoming>
      <bpmn:outgoing>Flow_1kslj45</bpmn:outgoing>
    </bpmn:serviceTask>
    <bpmn:sequenceFlow id="Flow_0csdaen" sourceRef="Activity_0cj0ef2" targetRef="Activity_0ttoxpx" />
    <bpmn:task id="Activity_0s5xebm" name="答复内容生成">
      <bpmn:incoming>Flow_1kslj45</bpmn:incoming>
      <bpmn:outgoing>Flow_0nbb420</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_1kslj45" sourceRef="Activity_0ttoxpx" targetRef="Activity_0s5xebm" />
    <bpmn:endEvent id="Event_0rl4sns" name="End">
      <bpmn:incoming>Flow_0nbb420</bpmn:incoming>
    </bpmn:endEvent>
    <bpmn:sequenceFlow id="Flow_0nbb420" sourceRef="Activity_0s5xebm" targetRef="Event_0rl4sns" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_7852">
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="352" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="358" y="276" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0cj0ef2_di" bpmnElement="Activity_0cj0ef2">
        <dc:Bounds x="540" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0ttoxpx_di" bpmnElement="Activity_0ttoxpx">
        <dc:Bounds x="750" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0s5xebm_di" bpmnElement="Activity_0s5xebm">
        <dc:Bounds x="990" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0rl4sns_di" bpmnElement="Event_0rl4sns">
        <dc:Bounds x="1192" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1200" y="283" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_1hjoyg4_di" bpmnElement="Flow_1hjoyg4">
        <di:waypoint x="388" y="258" />
        <di:waypoint x="540" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0csdaen_di" bpmnElement="Flow_0csdaen">
        <di:waypoint x="640" y="258" />
        <di:waypoint x="750" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1kslj45_di" bpmnElement="Flow_1kslj45">
        <di:waypoint x="850" y="258" />
        <di:waypoint x="990" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0nbb420_di" bpmnElement="Flow_0nbb420">
        <di:waypoint x="1090" y="258" />
        <di:waypoint x="1192" y="258" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
', 0, NULL, 0, NULL, NULL, NULL, '2025-12-23 19:44:29.812322+08', '2026-02-01 20:16:39.867138+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (99, 'Process_6253_5119', '1', 'Contract_6253_5119', 'Contract_Code_6253_5119', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="utf-8"?><bpmn:definitions targetNamespace="http://bpmn.io/schema/bpmn" id="bpmn-diagram" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="Process_6253_5119" name="Contract_6253_5119" isExecutable="true" sf:code="Contract_Code_6253_5119" sf:version="1"><bpmn:startEvent id="StartNode_2973" name="start" sf:code="Start"><bpmn:outgoing>Flow_1613</bpmn:outgoing></bpmn:startEvent><bpmn:task id="TaskNode_9684" name="Contract Draft" sf:code="task001"><bpmn:incoming>Flow_1613</bpmn:incoming><bpmn:outgoing>Flow_5422</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_4708" name="Approved by BA Manager" sf:code="task002"><bpmn:incoming>Flow_5422</bpmn:incoming><bpmn:outgoing>Flow_4799</bpmn:outgoing></bpmn:task><bpmn:parallelGateway id="GatewayNode_6544" name="And-Split" sf:code="andsplit001"><bpmn:incoming>Flow_4799</bpmn:incoming><bpmn:outgoing>Flow_3821</bpmn:outgoing><bpmn:outgoing>Flow_8234</bpmn:outgoing><bpmn:outgoing>Flow_6151</bpmn:outgoing></bpmn:parallelGateway><bpmn:task id="TaskNode_7965" name="Contract Department Review" sf:code="task010"><bpmn:incoming>Flow_3821</bpmn:incoming><bpmn:outgoing>Flow_2100</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_5024" name="Financial Department Review" sf:code="task020"><bpmn:incoming>Flow_8234</bpmn:incoming><bpmn:outgoing>Flow_5080</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_1700" name="Group Headquarters Review" sf:code="task030"><bpmn:incoming>Flow_6151</bpmn:incoming><bpmn:outgoing>Flow_4028</bpmn:outgoing></bpmn:task><bpmn:parallelGateway id="GatewayNode_4249" name="And-Join" sf:code="andjoin001"><bpmn:incoming>Flow_4028</bpmn:incoming><bpmn:incoming>Flow_5080</bpmn:incoming><bpmn:incoming>Flow_2100</bpmn:incoming><bpmn:outgoing>Flow_7595</bpmn:outgoing></bpmn:parallelGateway><bpmn:task id="TaskNode_2021" name="Contract Archived" sf:code="task007"><bpmn:incoming>Flow_7595</bpmn:incoming><bpmn:outgoing>Flow_3519</bpmn:outgoing></bpmn:task><bpmn:endEvent id="EndNode_1811" name="end" sf:code="End"><bpmn:incoming>Flow_3519</bpmn:incoming></bpmn:endEvent><bpmn:sequenceFlow id="Flow_1613" name="" sourceRef="StartNode_2973" targetRef="TaskNode_9684" /><bpmn:sequenceFlow id="Flow_5422" name="" sourceRef="TaskNode_9684" targetRef="TaskNode_4708" /><bpmn:sequenceFlow id="Flow_4799" name="" sourceRef="TaskNode_4708" targetRef="GatewayNode_6544" /><bpmn:sequenceFlow id="Flow_3821" name="" sourceRef="GatewayNode_6544" targetRef="TaskNode_7965" /><bpmn:sequenceFlow id="Flow_8234" name="" sourceRef="GatewayNode_6544" targetRef="TaskNode_5024" /><bpmn:sequenceFlow id="Flow_6151" name="" sourceRef="GatewayNode_6544" targetRef="TaskNode_1700" /><bpmn:sequenceFlow id="Flow_4028" name="" sourceRef="TaskNode_1700" targetRef="GatewayNode_4249" /><bpmn:sequenceFlow id="Flow_5080" name="" sourceRef="TaskNode_5024" targetRef="GatewayNode_4249" /><bpmn:sequenceFlow id="Flow_2100" name="" sourceRef="TaskNode_7965" targetRef="GatewayNode_4249" /><bpmn:sequenceFlow id="Flow_7595" name="" sourceRef="GatewayNode_4249" targetRef="TaskNode_2021" /><bpmn:sequenceFlow id="Flow_3519" name="" sourceRef="TaskNode_2021" targetRef="EndNode_1811" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100"><bpmndi:BPMNShape id="BPMNShape_lgsh2m1_di" bpmnElement="StartNode_2973"><dc:Bounds height="36" width="36" x="240" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_tt8mbf8_di" bpmnElement="TaskNode_9684"><dc:Bounds height="80" width="100" x="356" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_ph2z1ff_di" bpmnElement="TaskNode_4708"><dc:Bounds height="80" width="100" x="536" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_wcf5l7o_di" bpmnElement="GatewayNode_6544"><dc:Bounds height="36" width="36" x="716" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_xey34to_di" bpmnElement="TaskNode_7965"><dc:Bounds height="80" width="100" x="832" y="390" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_xfvgg63_di" bpmnElement="TaskNode_5024"><dc:Bounds height="80" width="100" x="832" y="230" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_mbsegyo_di" bpmnElement="TaskNode_1700"><dc:Bounds height="80" width="100" x="832" y="70" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_8i0tohl_di" bpmnElement="GatewayNode_4249"><dc:Bounds height="36" width="36" x="1012" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_ud0bsvz_di" bpmnElement="TaskNode_2021"><dc:Bounds height="80" width="100" x="1128" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_grtr4ww_di" bpmnElement="EndNode_1811"><dc:Bounds height="36" width="36" x="1308" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Flow_1613_di" bpmnElement="Flow_1613"><di:waypoint x="276" y="198" /><di:waypoint x="356" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_5422_di" bpmnElement="Flow_5422"><di:waypoint x="456" y="198" /><di:waypoint x="536" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_4799_di" bpmnElement="Flow_4799"><di:waypoint x="636" y="198" /><di:waypoint x="716" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_3821_di" bpmnElement="Flow_3821"><di:waypoint x="734" y="216" /><di:waypoint x="734" y="430" /><di:waypoint x="832" y="430" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_8234_di" bpmnElement="Flow_8234"><di:waypoint x="734" y="216" /><di:waypoint x="734" y="270" /><di:waypoint x="832" y="270" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_6151_di" bpmnElement="Flow_6151"><di:waypoint x="734" y="180" /><di:waypoint x="734" y="110" /><di:waypoint x="832" y="110" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_4028_di" bpmnElement="Flow_4028"><di:waypoint x="932" y="110" /><di:waypoint x="1030" y="110" /><di:waypoint x="1030" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_5080_di" bpmnElement="Flow_5080"><di:waypoint x="932" y="270" /><di:waypoint x="1030" y="270" /><di:waypoint x="1030" y="216" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_2100_di" bpmnElement="Flow_2100"><di:waypoint x="932" y="430" /><di:waypoint x="1030" y="430" /><di:waypoint x="1030" y="216" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_7595_di" bpmnElement="Flow_7595"><di:waypoint x="1048" y="198" /><di:waypoint x="1128" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_3519_di" bpmnElement="Flow_3519"><di:waypoint x="1228" y="198" /><di:waypoint x="1308" y="198" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, 0, NULL, NULL, NULL, '2025-12-23 17:08:00.73557+08', '2025-12-23 17:08:00.73557+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (105, 'Process_2917_8466', '1', 'AskforLeave_2917_8466', 'AskforLeave_Code_2917_8466', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="utf-8"?><bpmn:definitions targetNamespace="http://bpmn.io/schema/bpmn" id="bpmn-diagram" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="Process_2917_8466" name="AskforLeave_2917_8466" isExecutable="true" sf:code="AskforLeave_Code_2917_8466" sf:version="1"><bpmn:startEvent id="StartNode_5950" name="start" sf:code="Start"><bpmn:outgoing>Flow_6619</bpmn:outgoing></bpmn:startEvent><bpmn:task id="TaskNode_8334" name="Apply Submit" sf:code="task001"><bpmn:incoming>Flow_6619</bpmn:incoming><bpmn:outgoing>Flow_1338</bpmn:outgoing></bpmn:task><bpmn:exclusiveGateway id="GatewayNode_3913" name="XOr-Split" sf:code="xorsplit001"><bpmn:incoming>Flow_1338</bpmn:incoming><bpmn:outgoing>Flow_4054</bpmn:outgoing><bpmn:outgoing>Flow_3197</bpmn:outgoing></bpmn:exclusiveGateway><bpmn:task id="TaskNode_7348" name="Dept Manager Approval" sf:code="task010"><bpmn:incoming>Flow_4054</bpmn:incoming><bpmn:outgoing>Flow_8562</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_5353" name="CEO Approval" sf:code="task020"><bpmn:incoming>Flow_3197</bpmn:incoming><bpmn:outgoing>Flow_5365</bpmn:outgoing></bpmn:task><bpmn:exclusiveGateway id="GatewayNode_4119" name="XOr-Join" sf:code="xorjoin001"><bpmn:incoming>Flow_5365</bpmn:incoming><bpmn:incoming>Flow_8562</bpmn:incoming><bpmn:outgoing>Flow_5019</bpmn:outgoing></bpmn:exclusiveGateway><bpmn:task id="TaskNode_7394" name="HR Approval" sf:code="task100"><bpmn:incoming>Flow_5019</bpmn:incoming><bpmn:outgoing>Flow_7020</bpmn:outgoing></bpmn:task><bpmn:endEvent id="EndNode_2549" name="end" sf:code="End"><bpmn:incoming>Flow_7020</bpmn:incoming></bpmn:endEvent><bpmn:sequenceFlow id="Flow_6619" name="" sourceRef="StartNode_5950" targetRef="TaskNode_8334" /><bpmn:sequenceFlow id="Flow_1338" name="" sourceRef="TaskNode_8334" targetRef="GatewayNode_3913" /><bpmn:sequenceFlow id="Flow_4054" name="days&lt;3" sourceRef="GatewayNode_3913" targetRef="TaskNode_7348"><bpmn:conditionExpression>days&lt;3</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_3197" name="days&gt;=3" sourceRef="GatewayNode_3913" targetRef="TaskNode_5353"><bpmn:conditionExpression>days&gt;=3</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_5365" name="" sourceRef="TaskNode_5353" targetRef="GatewayNode_4119" /><bpmn:sequenceFlow id="Flow_8562" name="" sourceRef="TaskNode_7348" targetRef="GatewayNode_4119" /><bpmn:sequenceFlow id="Flow_5019" name="" sourceRef="GatewayNode_4119" targetRef="TaskNode_7394" /><bpmn:sequenceFlow id="Flow_7020" name="" sourceRef="TaskNode_7394" targetRef="EndNode_2549" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_100"><bpmndi:BPMNShape id="BPMNShape_s7yuo2x_di" bpmnElement="StartNode_5950"><dc:Bounds height="36" width="36" x="240" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_lxgbfyw_di" bpmnElement="TaskNode_8334"><dc:Bounds height="80" width="100" x="356" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_g7b4l81_di" bpmnElement="GatewayNode_3913"><dc:Bounds height="36" width="36" x="536" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_6ushi4y_di" bpmnElement="TaskNode_7348"><dc:Bounds height="80" width="100" x="652" y="230" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_ifv0znh_di" bpmnElement="TaskNode_5353"><dc:Bounds height="80" width="100" x="652" y="70" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_fii39ks_di" bpmnElement="GatewayNode_4119"><dc:Bounds height="36" width="36" x="832" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_jaej81x_di" bpmnElement="TaskNode_7394"><dc:Bounds height="80" width="100" x="948" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_9cbwh9z_di" bpmnElement="EndNode_2549"><dc:Bounds height="36" width="36" x="1128" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Flow_6619_di" bpmnElement="Flow_6619"><di:waypoint x="276" y="198" /><di:waypoint x="356" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_1338_di" bpmnElement="Flow_1338"><di:waypoint x="456" y="198" /><di:waypoint x="536" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_4054_di" bpmnElement="Flow_4054"><di:waypoint x="554" y="216" /><di:waypoint x="554" y="270" /><di:waypoint x="652" y="270" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_3197_di" bpmnElement="Flow_3197"><di:waypoint x="554" y="180" /><di:waypoint x="554" y="110" /><di:waypoint x="652" y="110" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_5365_di" bpmnElement="Flow_5365"><di:waypoint x="752" y="110" /><di:waypoint x="850" y="110" /><di:waypoint x="850" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_8562_di" bpmnElement="Flow_8562"><di:waypoint x="752" y="270" /><di:waypoint x="850" y="270" /><di:waypoint x="850" y="216" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_5019_di" bpmnElement="Flow_5019"><di:waypoint x="868" y="198" /><di:waypoint x="948" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_7020_di" bpmnElement="Flow_7020"><di:waypoint x="1048" y="198" /><di:waypoint x="1128" y="198" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, 0, NULL, NULL, NULL, '2025-12-23 18:17:41.269097+08', '2025-12-23 18:17:41.269097+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (106, 'TrainTicketBookingProcess_ie1a', '1', '火车订票流程', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="TrainTicketBookingProcess_ie1a" name="火车订票流程" isExecutable="false"><bpmn:startEvent id="AStartEvent" name="开始订票" /><bpmn:userTask id="AUserTask_SelectTrip" name="选择出发地、目的地和日期" /><bpmn:serviceTask id="AServiceTask_CheckAvailability" name="查询车票余票" /><bpmn:exclusiveGateway id="AExclusiveGateway_HasTickets" name="是否有余票" /><bpmn:userTask id="AUserTask_PassengerInfo" name="填写乘客信息" /><bpmn:userTask id="AUserTask_Payment" name="支付订单" /><bpmn:serviceTask id="AServiceTask_GenerateTicket" name="生成电子车票" /><bpmn:endEvent id="AEndEvent_Success" name="订票成功" /><bpmn:endEvent id="AEndEvent_Failed" name="订票失败" /><bpmn:sequenceFlow id="Flow_AStartEvent_AUserTask_SelectTrip" sourceRef="AStartEvent" targetRef="AUserTask_SelectTrip" /><bpmn:sequenceFlow id="Flow_AUserTask_SelectTrip_AServiceTask_CheckAvailability" sourceRef="AUserTask_SelectTrip" targetRef="AServiceTask_CheckAvailability" /><bpmn:sequenceFlow id="Flow_AServiceTask_CheckAvailability_AExclusiveGateway_HasTickets" sourceRef="AServiceTask_CheckAvailability" targetRef="AExclusiveGateway_HasTickets" /><bpmn:sequenceFlow id="Flow_AExclusiveGateway_HasTickets_AUserTask_PassengerInfo" sourceRef="AExclusiveGateway_HasTickets" targetRef="AUserTask_PassengerInfo"><bpmn:conditionExpression>余票 &gt; 0</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_AExclusiveGateway_HasTickets_AEndEvent_Failed" sourceRef="AExclusiveGateway_HasTickets" targetRef="AEndEvent_Failed"><bpmn:conditionExpression>余票 == 0</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_AUserTask_PassengerInfo_AUserTask_Payment" sourceRef="AUserTask_PassengerInfo" targetRef="AUserTask_Payment" /><bpmn:sequenceFlow id="Flow_AUserTask_Payment_AServiceTask_GenerateTicket" sourceRef="AUserTask_Payment" targetRef="AServiceTask_GenerateTicket" /><bpmn:sequenceFlow id="Flow_AServiceTask_GenerateTicket_AEndEvent_Success" sourceRef="AServiceTask_GenerateTicket" targetRef="AEndEvent_Success" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="TrainTicketBookingProcess_ie1a"><bpmndi:BPMNShape id="Shape_AStartEvent" bpmnElement="AStartEvent"><dc:Bounds x="150" y="162" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AUserTask_SelectTrip" bpmnElement="AUserTask_SelectTrip"><dc:Bounds x="350" y="140" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AServiceTask_CheckAvailability" bpmnElement="AServiceTask_CheckAvailability"><dc:Bounds x="550" y="140" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AExclusiveGateway_HasTickets" bpmnElement="AExclusiveGateway_HasTickets"><dc:Bounds x="750" y="162" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AUserTask_PassengerInfo" bpmnElement="AUserTask_PassengerInfo"><dc:Bounds x="950" y="301" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AUserTask_Payment" bpmnElement="AUserTask_Payment"><dc:Bounds x="1150" y="301" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AServiceTask_GenerateTicket" bpmnElement="AServiceTask_GenerateTicket"><dc:Bounds x="1350" y="301" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent_Success" bpmnElement="AEndEvent_Success"><dc:Bounds x="1550" y="323" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent_Failed" bpmnElement="AEndEvent_Failed"><dc:Bounds x="950" y="140" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AStartEvent_AUserTask_SelectTrip" bpmnElement="Flow_AStartEvent_AUserTask_SelectTrip"><di:waypoint x="186" y="180" /><di:waypoint x="236" y="180" /><di:waypoint x="236" y="180" /><di:waypoint x="350" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AUserTask_SelectTrip_AServiceTask_CheckAvailability" bpmnElement="Flow_AUserTask_SelectTrip_AServiceTask_CheckAvailability"><di:waypoint x="450" y="180" /><di:waypoint x="500" y="180" /><di:waypoint x="550" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AServiceTask_CheckAvailability_AExclusiveGateway_HasTickets" bpmnElement="Flow_AServiceTask_CheckAvailability_AExclusiveGateway_HasTickets"><di:waypoint x="650" y="180" /><di:waypoint x="700" y="180" /><di:waypoint x="700" y="180" /><di:waypoint x="750" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AExclusiveGateway_HasTickets_AUserTask_PassengerInfo" bpmnElement="Flow_AExclusiveGateway_HasTickets_AUserTask_PassengerInfo"><di:waypoint x="786" y="180" /><di:waypoint x="836" y="180" /><di:waypoint x="836" y="341" /><di:waypoint x="950" y="341" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AExclusiveGateway_HasTickets_AEndEvent_Failed" bpmnElement="Flow_AExclusiveGateway_HasTickets_AEndEvent_Failed"><di:waypoint x="786" y="180" /><di:waypoint x="836" y="180" /><di:waypoint x="836" y="158" /><di:waypoint x="950" y="158" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AUserTask_PassengerInfo_AUserTask_Payment" bpmnElement="Flow_AUserTask_PassengerInfo_AUserTask_Payment"><di:waypoint x="1050" y="341" /><di:waypoint x="1100" y="341" /><di:waypoint x="1150" y="341" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AUserTask_Payment_AServiceTask_GenerateTicket" bpmnElement="Flow_AUserTask_Payment_AServiceTask_GenerateTicket"><di:waypoint x="1250" y="341" /><di:waypoint x="1300" y="341" /><di:waypoint x="1350" y="341" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AServiceTask_GenerateTicket_AEndEvent_Success" bpmnElement="Flow_AServiceTask_GenerateTicket_AEndEvent_Success"><di:waypoint x="1450" y="341" /><di:waypoint x="1500" y="341" /><di:waypoint x="1500" y="341" /><di:waypoint x="1550" y="341" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, 0, NULL, NULL, NULL, '2025-12-25 11:40:40.190986+08', '2025-12-25 11:40:40.190987+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (107, 'Process_6840', '1', 'Process_Name_6840', 'Process_Code_6840', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_6840" sf:code="Process_Code_6840" name="Process_Name_6840" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="StartEvent_1" name="Start">
      <bpmn:outgoing>Flow_1g65tfp</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:task id="Activity_1redo79" name="task-01">
      <bpmn:extensionElements>
        <sf:variables>
          <sf:variable name="ImageContent" type="Object" defaultValue="" direction="Output" isReferenced="false" isRequired="true" />
        </sf:variables>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_1g65tfp</bpmn:incoming>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_1g65tfp" sourceRef="StartEvent_1" targetRef="Activity_1redo79" />
    <bpmn:endEvent id="Event_0w25mvl" name="End" />
    <bpmn:serviceTask id="Activity_0aijq2e">
      <bpmn:extensionElements>
        <sf:aiServices>
          <sf:aiService type="RAG" />
        </sf:aiServices>
      </bpmn:extensionElements>
    </bpmn:serviceTask>
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_6840">
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1redo79_di" bpmnElement="Activity_1redo79">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0w25mvl_di" bpmnElement="Event_0w25mvl">
        <dc:Bounds x="902" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="910" y="283" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0aijq2e_di" bpmnElement="Activity_0aijq2e">
        <dc:Bounds x="690" y="218" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_1g65tfp_di" bpmnElement="Flow_1g65tfp">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             ', 0, NULL, 0, NULL, NULL, NULL, '2025-12-26 20:40:13.318925+08', '2025-12-25 15:25:48.63497+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (108, 'Process_8181', '1', 'Process_Image_Classification_1001', 'Process_Code_8181', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_8181" sf:code="Process_Code_8181" name="Process_Image_Classification_1001" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="StartEvent_1" name="Start">
      <bpmn:outgoing>Flow_08izj8p</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:serviceTask id="Activity_0qik8mv" name="image classify">
      <bpmn:extensionElements>
        <sf:aiServices>
          <sf:aiService configUUID="65d4a527-5899-4860-e1ed-70479edf444f" type="LLM" />
        </sf:aiServices>
        <sf:variables>
          <sf:variable name="ImageContent" type="Object" defaultValue="" direction="Input" isReferenced="true" isRequired="true">
            <sf:varRefDetail sourceRef="Activity_1n3nw6h" variableName="ImageContent" />
          </sf:variable>
          <sf:variable name="pet" type="String" defaultValue="" direction="Output" isReferenced="false" isRequired="true" />
        </sf:variables>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_05o0pye</bpmn:incoming>
      <bpmn:outgoing>Flow_080jpn4</bpmn:outgoing>
    </bpmn:serviceTask>
    <bpmn:sequenceFlow id="Flow_08izj8p" sourceRef="StartEvent_1" targetRef="Activity_1n3nw6h" />
    <bpmn:sequenceFlow id="Flow_05o0pye" sourceRef="Activity_1n3nw6h" targetRef="Activity_0qik8mv" />
    <bpmn:endEvent id="Event_1ecoeyy" name="End">
      <bpmn:incoming>Flow_1x8j1i4</bpmn:incoming>
      <bpmn:incoming>Flow_1mcbtkl</bpmn:incoming>
    </bpmn:endEvent>
    <bpmn:exclusiveGateway id="Gateway_0tw9dau">
      <bpmn:incoming>Flow_080jpn4</bpmn:incoming>
      <bpmn:outgoing>Flow_0vwlmab</bpmn:outgoing>
      <bpmn:outgoing>Flow_1p7o9sh</bpmn:outgoing>
    </bpmn:exclusiveGateway>
    <bpmn:sequenceFlow id="Flow_080jpn4" sourceRef="Activity_0qik8mv" targetRef="Gateway_0tw9dau" />
    <bpmn:task id="Activity_059c7sx" name="buy cat food">
      <bpmn:incoming>Flow_0vwlmab</bpmn:incoming>
      <bpmn:outgoing>Flow_1x8j1i4</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_0vwlmab" name="pet == &#34;cat&#34;" sourceRef="Gateway_0tw9dau" targetRef="Activity_059c7sx">
      <bpmn:conditionExpression>(pet == "cat")</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:task id="Activity_0pvot1r" name="buy dog food">
      <bpmn:incoming>Flow_1p7o9sh</bpmn:incoming>
      <bpmn:outgoing>Flow_1mcbtkl</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_1p7o9sh" name="pet == &#34;dog&#34;" sourceRef="Gateway_0tw9dau" targetRef="Activity_0pvot1r">
      <bpmn:conditionExpression>(pet == "dog")</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:sequenceFlow id="Flow_1x8j1i4" sourceRef="Activity_059c7sx" targetRef="Event_1ecoeyy" />
    <bpmn:sequenceFlow id="Flow_1mcbtkl" sourceRef="Activity_0pvot1r" targetRef="Event_1ecoeyy" />
    <bpmn:userTask id="Activity_1n3nw6h" name="upload animal image">
      <bpmn:extensionElements>
        <sf:variables>
          <sf:variable name="ImageContent" type="Object" defaultValue="" direction="Output" isReferenced="false" isRequired="true" />
        </sf:variables>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_08izj8p</bpmn:incoming>
      <bpmn:outgoing>Flow_05o0pye</bpmn:outgoing>
    </bpmn:userTask>
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_8181">
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="262" y="302" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="268" y="338" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0qik8mv_di" bpmnElement="Activity_0qik8mv">
        <dc:Bounds x="610" y="280" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1ecoeyy_di" bpmnElement="Event_1ecoeyy">
        <dc:Bounds x="1232" y="302" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1240" y="345" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_0tw9dau_di" bpmnElement="Gateway_0tw9dau" isMarkerVisible="true">
        <dc:Bounds x="815" y="295" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_059c7sx_di" bpmnElement="Activity_059c7sx">
        <dc:Bounds x="970" y="180" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0pvot1r_di" bpmnElement="Activity_0pvot1r">
        <dc:Bounds x="970" y="280" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1a1eas5_di" bpmnElement="Activity_1n3nw6h">
        <dc:Bounds x="410" y="280" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_08izj8p_di" bpmnElement="Flow_08izj8p">
        <di:waypoint x="298" y="320" />
        <di:waypoint x="410" y="320" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_05o0pye_di" bpmnElement="Flow_05o0pye">
        <di:waypoint x="510" y="320" />
        <di:waypoint x="610" y="320" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_080jpn4_di" bpmnElement="Flow_080jpn4">
        <di:waypoint x="710" y="320" />
        <di:waypoint x="815" y="320" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0vwlmab_di" bpmnElement="Flow_0vwlmab">
        <di:waypoint x="840" y="295" />
        <di:waypoint x="840" y="220" />
        <di:waypoint x="970" y="220" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="827" y="255" width="57" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1p7o9sh_di" bpmnElement="Flow_1p7o9sh">
        <di:waypoint x="865" y="320" />
        <di:waypoint x="970" y="320" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="888" y="302" width="61" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1x8j1i4_di" bpmnElement="Flow_1x8j1i4">
        <di:waypoint x="1070" y="220" />
        <di:waypoint x="1151" y="220" />
        <di:waypoint x="1151" y="320" />
        <di:waypoint x="1232" y="320" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1mcbtkl_di" bpmnElement="Flow_1mcbtkl">
        <di:waypoint x="1070" y="320" />
        <di:waypoint x="1232" y="320" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
', 0, NULL, 0, NULL, NULL, NULL, '2025-12-26 10:37:15.52452+08', '2025-12-26 10:37:15.52452+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (109, 'Process_6824_9519', '1', '_6824_9519', '_Code_6824_9519', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 0, NULL, 0, NULL, NULL, NULL, '2026-01-15 12:29:51.35129+08', NULL, NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (185, 'Process_3334', '1', 'Process_Name_3334', 'Process_Code_3334', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_3334" sf:code="Process_Code_3334" name="Process_Name_3334" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="StartEvent_1" name="Start">
      <bpmn:outgoing>Flow_00jvcu4</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:task id="Activity_12jz8r5" name="task">
      <bpmn:incoming>Flow_00jvcu4</bpmn:incoming>
      <bpmn:outgoing>Flow_0oz0lqk</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_00jvcu4" sourceRef="StartEvent_1" targetRef="Activity_12jz8r5" />
    <bpmn:task id="Activity_18nnc1r" name="ophiasdf">
      <bpmn:incoming>Flow_0oz0lqk</bpmn:incoming>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_0oz0lqk" sourceRef="Activity_12jz8r5" targetRef="Activity_18nnc1r" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_3334">
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_12jz8r5_di" bpmnElement="Activity_12jz8r5">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_18nnc1r_di" bpmnElement="Activity_18nnc1r">
        <dc:Bounds x="660" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_00jvcu4_di" bpmnElement="Flow_00jvcu4">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0oz0lqk_di" bpmnElement="Flow_0oz0lqk">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="660" y="258" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            ', 0, NULL, 0, NULL, NULL, NULL, '2026-01-15 20:58:37.201567+08', '2026-01-15 20:58:37.201567+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (243, 'process_4508', '1', 'LargeOrderProcess_1747', 'LargeOrderProcess_Code_1747', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?><bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd"><bpmn:process id="process_4508" name="LargeOrderProcess_1747" isExecutable="true" sf:code="LargeOrderProcess_Code_1747" sf:version="1"><bpmn:startEvent id="StartNode_8973" name="Start" sf:code="Start"><bpmn:outgoing>Flow_6009</bpmn:outgoing></bpmn:startEvent><bpmn:task id="TaskNode_2319" name="Large Order Received" sf:code="001"><bpmn:incoming>Flow_6009</bpmn:incoming><bpmn:outgoing>Flow_1467</bpmn:outgoing></bpmn:task><bpmn:parallelGateway id="GatewayNode_4947" name="AndSplit" sf:code="AS002"><bpmn:incoming>Flow_1467</bpmn:incoming><bpmn:outgoing>Flow_8456</bpmn:outgoing><bpmn:outgoing>Flow_7443</bpmn:outgoing><bpmn:outgoing>Flow_1438</bpmn:outgoing></bpmn:parallelGateway><bpmn:task id="TaskNode_9313" name="Engineering Review" sf:code="0011"><bpmn:incoming>Flow_8456</bpmn:incoming><bpmn:outgoing>Flow_9637</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_7317" name="Design Review" sf:code="0012"><bpmn:incoming>Flow_7443</bpmn:incoming><bpmn:outgoing>Flow_7281</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_6985" name="QA Review" sf:code="0013"><bpmn:incoming>Flow_1438</bpmn:incoming><bpmn:outgoing>Flow_9105</bpmn:outgoing></bpmn:task><bpmn:parallelGateway id="GatewayNode_6237" name="AndJoin" sf:code="AJ002"><bpmn:incoming>Flow_9105</bpmn:incoming><bpmn:incoming>Flow_7281</bpmn:incoming><bpmn:incoming>Flow_9637</bpmn:incoming><bpmn:outgoing>Flow_1644</bpmn:outgoing></bpmn:parallelGateway><bpmn:task id="TaskNode_2194" name="Management Approve" sf:code="007"><bpmn:incoming>Flow_1644</bpmn:incoming><bpmn:outgoing>Flow_1100</bpmn:outgoing></bpmn:task><bpmn:endEvent id="EndNode_5515" name="End" sf:code="End"><bpmn:incoming>Flow_1100</bpmn:incoming></bpmn:endEvent><bpmn:sequenceFlow id="Flow_6009" name="" sourceRef="StartNode_8973" targetRef="TaskNode_2319" /><bpmn:sequenceFlow id="Flow_1467" name="" sourceRef="TaskNode_2319" targetRef="GatewayNode_4947" /><bpmn:sequenceFlow id="Flow_8456" name="" sourceRef="GatewayNode_4947" targetRef="TaskNode_9313" /><bpmn:sequenceFlow id="Flow_7443" name="" sourceRef="GatewayNode_4947" targetRef="TaskNode_7317" /><bpmn:sequenceFlow id="Flow_1438" name="" sourceRef="GatewayNode_4947" targetRef="TaskNode_6985" /><bpmn:sequenceFlow id="Flow_9105" name="" sourceRef="TaskNode_6985" targetRef="GatewayNode_6237" /><bpmn:sequenceFlow id="Flow_7281" name="" sourceRef="TaskNode_7317" targetRef="GatewayNode_6237" /><bpmn:sequenceFlow id="Flow_9637" name="" sourceRef="TaskNode_9313" targetRef="GatewayNode_6237" /><bpmn:sequenceFlow id="Flow_1644" name="" sourceRef="GatewayNode_6237" targetRef="TaskNode_2194" /><bpmn:sequenceFlow id="Flow_1100" name="" sourceRef="TaskNode_2194" targetRef="EndNode_5515" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="process_4508"><bpmndi:BPMNShape id="BPMNShape_urjq6pq_di" bpmnElement="StartNode_8973"><dc:Bounds height="36" width="36" x="240" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_tbxsxz2_di" bpmnElement="TaskNode_2319"><dc:Bounds height="80" width="100" x="356" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_4grv1zs_di" bpmnElement="GatewayNode_4947"><dc:Bounds height="36" width="36" x="536" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_cdfcwak_di" bpmnElement="TaskNode_9313"><dc:Bounds height="80" width="100" x="652" y="390" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_yk4gh2p_di" bpmnElement="TaskNode_7317"><dc:Bounds height="80" width="100" x="652" y="230" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_97hfxft_di" bpmnElement="TaskNode_6985"><dc:Bounds height="80" width="100" x="652" y="70" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_acfzqq5_di" bpmnElement="GatewayNode_6237"><dc:Bounds height="36" width="36" x="832" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_eqhdgds_di" bpmnElement="TaskNode_2194"><dc:Bounds height="80" width="100" x="948" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_joz88do_di" bpmnElement="EndNode_5515"><dc:Bounds height="36" width="36" x="1128" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Flow_6009_di" bpmnElement="Flow_6009"><di:waypoint x="276" y="198" /><di:waypoint x="356" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_1467_di" bpmnElement="Flow_1467"><di:waypoint x="456" y="198" /><di:waypoint x="536" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_8456_di" bpmnElement="Flow_8456"><di:waypoint x="554" y="216" /><di:waypoint x="554" y="430" /><di:waypoint x="652" y="430" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_7443_di" bpmnElement="Flow_7443"><di:waypoint x="554" y="216" /><di:waypoint x="554" y="270" /><di:waypoint x="652" y="270" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_1438_di" bpmnElement="Flow_1438"><di:waypoint x="554" y="180" /><di:waypoint x="554" y="110" /><di:waypoint x="652" y="110" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_9105_di" bpmnElement="Flow_9105"><di:waypoint x="752" y="110" /><di:waypoint x="850" y="110" /><di:waypoint x="850" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_7281_di" bpmnElement="Flow_7281"><di:waypoint x="752" y="270" /><di:waypoint x="850" y="270" /><di:waypoint x="850" y="216" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_9637_di" bpmnElement="Flow_9637"><di:waypoint x="752" y="430" /><di:waypoint x="850" y="430" /><di:waypoint x="850" y="216" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_1644_di" bpmnElement="Flow_1644"><di:waypoint x="868" y="198" /><di:waypoint x="948" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_1100_di" bpmnElement="Flow_1100"><di:waypoint x="1048" y="198" /><di:waypoint x="1128" y="198" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, 0, NULL, NULL, NULL, '2026-01-18 12:00:54.492875+08', '2026-01-18 12:00:54.492929+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (244, 'Process_1671', '1', 'Process_小程序智能客服', 'Process_Code_1671', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_1671" sf:code="Process_Code_1671" name="Process_小程序智能客服" isExecutable="true" sf:version="1">
    <bpmn:task id="Activity_1xm9cg4" name="接收客户的提问">
      <bpmn:extensionElements>
        <sf:variables>
          <sf:variable name="user_message" type="String" defaultValue="" direction="Output" isReferenced="false" isRequired="true" />
        </sf:variables>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_0n48hom</bpmn:incoming>
      <bpmn:outgoing>Flow_0hfhj2p</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_0n48hom" sourceRef="StartEvent_1" targetRef="Activity_1xm9cg4" />
    <bpmn:endEvent id="Event_0jv1jk5" name="End">
      <bpmn:incoming>Flow_016alj5</bpmn:incoming>
    </bpmn:endEvent>
    <bpmn:sequenceFlow id="Flow_016alj5" sourceRef="Activity_0qbx206" targetRef="Event_0jv1jk5" />
    <bpmn:serviceTask id="Activity_0fqciiy" name="检索知识库, 并回答">
      <bpmn:extensionElements>
        <sf:aiServices>
          <sf:aiService type="RAG" />
        </sf:aiServices>
        <sf:variables>
          <sf:variable name="user_message" type="String" defaultValue="" direction="Input" isReferenced="true" isRequired="true">
            <sf:varRefDetail sourceRef="Activity_1xm9cg4" variableName="user_message" />
          </sf:variable>
          <sf:variable name="ai_response" type="String" defaultValue="" direction="Output" isReferenced="false" isRequired="true" />
        </sf:variables>
        <sf:sections>
          <sf:section name="myProperties">{"isNotifyClient":true}</sf:section>
        </sf:sections>
        <sf:performers />
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_0hfhj2p</bpmn:incoming>
      <bpmn:outgoing>Flow_0xtfyws</bpmn:outgoing>
    </bpmn:serviceTask>
    <bpmn:sequenceFlow id="Flow_0hfhj2p" sourceRef="Activity_1xm9cg4" targetRef="Activity_0fqciiy" />
    <bpmn:startEvent id="StartEvent_1" name="Start">
      <bpmn:outgoing>Flow_0n48hom</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:serviceTask id="Activity_0qbx206" name="保存会话记录">
      <bpmn:extensionElements>
        <sf:variables>
          <sf:variable name="ai_response" type="String" defaultValue="" direction="Input" isReferenced="true" isRequired="true">
            <sf:varRefDetail sourceRef="Activity_0fqciiy" variableName="ai_response" />
          </sf:variable>
          <sf:variable name="user_message" type="String" defaultValue="" direction="Input" isReferenced="true" isRequired="true">
            <sf:varRefDetail sourceRef="Activity_1xm9cg4" variableName="user_message" />
          </sf:variable>
          <sf:variable name="customer" type="Object" defaultValue="" direction="Input" isReferenced="true" isRequired="true">
            <sf:varRefDetail sourceRef="Activity_1no2857" variableName="customer" />
          </sf:variable>
        </sf:variables>
        <sf:services>
          <sf:service methodType="LocalService" argus="user_message, ai_response,customer" expression="Slickflow.Module.External.Customer.ConversationService" />
        </sf:services>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_02u3qps</bpmn:incoming>
      <bpmn:outgoing>Flow_016alj5</bpmn:outgoing>
    </bpmn:serviceTask>
    <bpmn:sequenceFlow id="Flow_02u3qps" sourceRef="Activity_0l4hsqh" targetRef="Activity_0qbx206" />
    <bpmn:serviceTask id="Activity_0l4hsqh" name="保存客户数据">
      <bpmn:extensionElements>
        <sf:variables>
          <sf:variable name="customer" type="Object" defaultValue="" direction="Input" isReferenced="true" isRequired="true">
            <sf:varRefDetail sourceRef="Activity_1no2857" variableName="customer" />
          </sf:variable>
        </sf:variables>
        <sf:services>
          <sf:service methodType="LocalService" argus="customer" expression="Slickflow.Module.External.Customer.CustomerSaveService" />
        </sf:services>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_1wa2nqs</bpmn:incoming>
      <bpmn:outgoing>Flow_02u3qps</bpmn:outgoing>
    </bpmn:serviceTask>
    <bpmn:sequenceFlow id="Flow_0xtfyws" sourceRef="Activity_0fqciiy" targetRef="Activity_10qz9dr" />
    <bpmn:serviceTask id="Activity_10qz9dr" name="提取客户联系方式">
      <bpmn:extensionElements>
        <sf:services>
          <sf:service methodType="LocalService" argus="user_message" expression="Slickflow.Module.External.Customer.CustomerExtractService" />
        </sf:services>
        <sf:variables>
          <sf:variable name="user_message" type="String" defaultValue="" direction="Input" isReferenced="true" isRequired="true">
            <sf:varRefDetail sourceRef="Activity_1xm9cg4" variableName="user_message" />
          </sf:variable>
          <sf:variable name="customer" type="Object" defaultValue="" direction="Output" isReferenced="false" isRequired="true" />
        </sf:variables>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_0xtfyws</bpmn:incoming>
      <bpmn:outgoing>Flow_1wa2nqs</bpmn:outgoing>
    </bpmn:serviceTask>
    <bpmn:sequenceFlow id="Flow_1wa2nqs" sourceRef="Activity_10qz9dr" targetRef="Activity_0l4hsqh" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_1671">
      <bpmndi:BPMNShape id="Activity_1xm9cg4_di" bpmnElement="Activity_1xm9cg4">
        <dc:Bounds x="260" y="280" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0jv1jk5_di" bpmnElement="Event_0jv1jk5">
        <dc:Bounds x="1232" y="302" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1240" y="345" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0fqciiy_di" bpmnElement="Activity_0fqciiy">
        <dc:Bounds x="470" y="280" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0dv2tay_di" bpmnElement="StartEvent_1">
        <dc:Bounds x="122" y="302" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="128" y="338" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0eufl05_di" bpmnElement="Activity_0qbx206">
        <dc:Bounds x="1020" y="280" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0g6189j_di" bpmnElement="Activity_0l4hsqh">
        <dc:Bounds x="850" y="280" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1vbpeb9_di" bpmnElement="Activity_10qz9dr">
        <dc:Bounds x="650" y="280" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_0n48hom_di" bpmnElement="Flow_0n48hom">
        <di:waypoint x="158" y="320" />
        <di:waypoint x="260" y="320" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_016alj5_di" bpmnElement="Flow_016alj5">
        <di:waypoint x="1120" y="320" />
        <di:waypoint x="1232" y="320" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0hfhj2p_di" bpmnElement="Flow_0hfhj2p">
        <di:waypoint x="360" y="320" />
        <di:waypoint x="470" y="320" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_02u3qps_di" bpmnElement="Flow_02u3qps">
        <di:waypoint x="950" y="320" />
        <di:waypoint x="1020" y="320" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0xtfyws_di" bpmnElement="Flow_0xtfyws">
        <di:waypoint x="570" y="320" />
        <di:waypoint x="650" y="320" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1wa2nqs_di" bpmnElement="Flow_1wa2nqs">
        <di:waypoint x="750" y="320" />
        <di:waypoint x="850" y="320" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
', 0, NULL, 0, NULL, NULL, NULL, '2026-02-04 19:04:35.228326+08', '2026-02-10 16:04:23.227793+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (246, 'Process_1279', '1', 'SequenceProcess-0001', 'Process_Code_1279', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_1279" sf:code="Process_Code_1279" name="SequenceProcess-0001" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="StartEvent_1" name="Start">
      <bpmn:outgoing>Flow_0rmykrv</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:sequenceFlow id="Flow_0rmykrv" sourceRef="StartEvent_1" targetRef="Activity_1ytz2d3" />
    <bpmn:sequenceFlow id="Flow_0yg1z7e" sourceRef="Activity_1ytz2d3" targetRef="Activity_12n8wtw" />
    <bpmn:sequenceFlow id="Flow_0dqrygk" sourceRef="Activity_12n8wtw" targetRef="Activity_1vbtv5u" />
    <bpmn:endEvent id="Event_1ingwkx" name="End">
      <bpmn:incoming>Flow_0mg0ug6</bpmn:incoming>
    </bpmn:endEvent>
    <bpmn:sequenceFlow id="Flow_0mg0ug6" sourceRef="Activity_1vbtv5u" targetRef="Event_1ingwkx" />
    <bpmn:serviceTask id="Activity_1ytz2d3" name="task-01">
      <bpmn:incoming>Flow_0rmykrv</bpmn:incoming>
      <bpmn:outgoing>Flow_0yg1z7e</bpmn:outgoing>
    </bpmn:serviceTask>
    <bpmn:serviceTask id="Activity_12n8wtw" name="task-02">
      <bpmn:incoming>Flow_0yg1z7e</bpmn:incoming>
      <bpmn:outgoing>Flow_0dqrygk</bpmn:outgoing>
    </bpmn:serviceTask>
    <bpmn:serviceTask id="Activity_1vbtv5u" name="task-03">
      <bpmn:incoming>Flow_0dqrygk</bpmn:incoming>
      <bpmn:outgoing>Flow_0mg0ug6</bpmn:outgoing>
    </bpmn:serviceTask>
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_1279">
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_1ingwkx_di" bpmnElement="Event_1ingwkx">
        <dc:Bounds x="982" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="990" y="283" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1o83heb_di" bpmnElement="Activity_1ytz2d3">
        <dc:Bounds x="500" y="218" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1f15o0f_di" bpmnElement="Activity_12n8wtw">
        <dc:Bounds x="660" y="218" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0k4hppq_di" bpmnElement="Activity_1vbtv5u">
        <dc:Bounds x="820" y="218" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_0rmykrv_di" bpmnElement="Flow_0rmykrv">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0yg1z7e_di" bpmnElement="Flow_0yg1z7e">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="660" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0dqrygk_di" bpmnElement="Flow_0dqrygk">
        <di:waypoint x="760" y="258" />
        <di:waypoint x="820" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0mg0ug6_di" bpmnElement="Flow_0mg0ug6">
        <di:waypoint x="920" y="258" />
        <di:waypoint x="982" y="258" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
                                                                                                                                                                                                                                                                                                                                                                                                                      ', 0, NULL, 0, NULL, NULL, NULL, '2026-01-24 05:03:56.666807+08', '2026-01-23 14:17:28.615253+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (247, 'process_3979', '1', 'process_111', 'process_111_code', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?><bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd"><bpmn:process id="process_3979" name="process_111" isExecutable="true" sf:code="process_111_code" sf:version="1"><bpmn:startEvent id="StartNode_6179" name="Start" sf:code="Start"><bpmn:outgoing>Flow_1055</bpmn:outgoing></bpmn:startEvent><bpmn:task id="TaskNode_8563" name="Task-001" sf:code="2k422f"><bpmn:incoming>Flow_1055</bpmn:incoming><bpmn:outgoing>Flow_2489</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_9847" name="Task-002" sf:code="t1paut"><bpmn:incoming>Flow_2489</bpmn:incoming><bpmn:outgoing>Flow_1703</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_2837" name="Task-003" sf:code="oz3jav"><bpmn:incoming>Flow_1703</bpmn:incoming><bpmn:outgoing>Flow_5743</bpmn:outgoing></bpmn:task><bpmn:endEvent id="EndNode_6769" name="End" sf:code="End"><bpmn:incoming>Flow_5743</bpmn:incoming></bpmn:endEvent><bpmn:sequenceFlow id="Flow_1055" name="" sourceRef="StartNode_6179" targetRef="TaskNode_8563" /><bpmn:sequenceFlow id="Flow_2489" name="" sourceRef="TaskNode_8563" targetRef="TaskNode_9847" /><bpmn:sequenceFlow id="Flow_1703" name="" sourceRef="TaskNode_9847" targetRef="TaskNode_2837" /><bpmn:sequenceFlow id="Flow_5743" name="" sourceRef="TaskNode_2837" targetRef="EndNode_6769" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="process_3979"><bpmndi:BPMNShape id="BPMNShape_9fx90z9_di" bpmnElement="StartNode_6179"><dc:Bounds height="36" width="36" x="240" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_xjplmlt_di" bpmnElement="TaskNode_8563"><dc:Bounds height="80" width="100" x="356" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_d9og258_di" bpmnElement="TaskNode_9847"><dc:Bounds height="80" width="100" x="536" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_8ci6xj8_di" bpmnElement="TaskNode_2837"><dc:Bounds height="80" width="100" x="716" y="158" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="BPMNShape_ogz327m_di" bpmnElement="EndNode_6769"><dc:Bounds height="36" width="36" x="896" y="180" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Flow_1055_di" bpmnElement="Flow_1055"><di:waypoint x="276" y="198" /><di:waypoint x="356" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_2489_di" bpmnElement="Flow_2489"><di:waypoint x="456" y="198" /><di:waypoint x="536" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_1703_di" bpmnElement="Flow_1703"><di:waypoint x="636" y="198" /><di:waypoint x="716" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_5743_di" bpmnElement="Flow_5743"><di:waypoint x="816" y="198" /><di:waypoint x="896" y="198" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, 0, NULL, NULL, NULL, '2026-01-23 17:21:36.212546+08', '2026-01-23 17:21:36.212688+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (248, 'Process_5390', '1', 'Process_AndSplit_AndJoin', 'Process_Code_5390', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_5390" sf:code="Process_Code_5390" name="Process_AndSplit_AndJoin" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="StartEvent_1" name="Start">
      <bpmn:outgoing>Flow_019wkoz</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:task id="Activity_06lc6kj" name="Task-01">
      <bpmn:incoming>Flow_019wkoz</bpmn:incoming>
      <bpmn:outgoing>Flow_0epa3rs</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_019wkoz" sourceRef="StartEvent_1" targetRef="Activity_06lc6kj" />
    <bpmn:sequenceFlow id="Flow_0epa3rs" sourceRef="Activity_06lc6kj" targetRef="Gateway_05sq571" />
    <bpmn:parallelGateway id="Gateway_05sq571">
      <bpmn:incoming>Flow_0epa3rs</bpmn:incoming>
      <bpmn:outgoing>Flow_0wwnomx</bpmn:outgoing>
      <bpmn:outgoing>Flow_0yohzql</bpmn:outgoing>
    </bpmn:parallelGateway>
    <bpmn:task id="Activity_0zu5kh0" name="Task-02">
      <bpmn:incoming>Flow_0wwnomx</bpmn:incoming>
      <bpmn:outgoing>Flow_0nt3lcv</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_0wwnomx" sourceRef="Gateway_05sq571" targetRef="Activity_0zu5kh0" />
    <bpmn:task id="Activity_0hwyuyj" name="Task-03">
      <bpmn:incoming>Flow_0yohzql</bpmn:incoming>
      <bpmn:outgoing>Flow_1eqy2rb</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_0yohzql" sourceRef="Gateway_05sq571" targetRef="Activity_0hwyuyj" />
    <bpmn:sequenceFlow id="Flow_0nt3lcv" sourceRef="Activity_0zu5kh0" targetRef="Gateway_1yl4snf" />
    <bpmn:parallelGateway id="Gateway_1yl4snf">
      <bpmn:incoming>Flow_0nt3lcv</bpmn:incoming>
      <bpmn:incoming>Flow_1eqy2rb</bpmn:incoming>
      <bpmn:outgoing>Flow_1xxo3zk</bpmn:outgoing>
    </bpmn:parallelGateway>
    <bpmn:sequenceFlow id="Flow_1eqy2rb" sourceRef="Activity_0hwyuyj" targetRef="Gateway_1yl4snf" />
    <bpmn:endEvent id="Event_0uvdyhv" name="End">
      <bpmn:incoming>Flow_1xxo3zk</bpmn:incoming>
    </bpmn:endEvent>
    <bpmn:sequenceFlow id="Flow_1xxo3zk" sourceRef="Gateway_1yl4snf" targetRef="Event_0uvdyhv" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_5390">
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="412" y="240" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_06lc6kj_di" bpmnElement="Activity_06lc6kj">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_0wn8jpr_di" bpmnElement="Gateway_05sq571">
        <dc:Bounds x="655" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0zu5kh0_di" bpmnElement="Activity_0zu5kh0">
        <dc:Bounds x="760" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0hwyuyj_di" bpmnElement="Activity_0hwyuyj">
        <dc:Bounds x="760" y="330" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_0c4086u_di" bpmnElement="Gateway_1yl4snf">
        <dc:Bounds x="915" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0uvdyhv_di" bpmnElement="Event_0uvdyhv">
        <dc:Bounds x="1022" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1030" y="283" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_019wkoz_di" bpmnElement="Flow_019wkoz">
        <di:waypoint x="448" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0epa3rs_di" bpmnElement="Flow_0epa3rs">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="655" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0wwnomx_di" bpmnElement="Flow_0wwnomx">
        <di:waypoint x="705" y="258" />
        <di:waypoint x="760" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0yohzql_di" bpmnElement="Flow_0yohzql">
        <di:waypoint x="680" y="283" />
        <di:waypoint x="680" y="370" />
        <di:waypoint x="760" y="370" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0nt3lcv_di" bpmnElement="Flow_0nt3lcv">
        <di:waypoint x="860" y="258" />
        <di:waypoint x="915" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1eqy2rb_di" bpmnElement="Flow_1eqy2rb">
        <di:waypoint x="860" y="370" />
        <di:waypoint x="940" y="370" />
        <di:waypoint x="940" y="283" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1xxo3zk_di" bpmnElement="Flow_1xxo3zk">
        <di:waypoint x="965" y="258" />
        <di:waypoint x="1022" y="258" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
', 0, NULL, 0, NULL, NULL, NULL, '2026-01-23 17:36:57.213981+08', '2026-01-23 17:36:57.213981+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (249, 'P001_uvqy', '1', '血常规检查流程', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL">
  <bpmn:process id="P001_uvqy" name="血常规检查流程" isExecutable="false">
    <bpmn:startEvent id="AStartEvent" name="开始检查" />
    <bpmn:userTask id="AActivity_PatientRegistration" name="患者登记" />
    <bpmn:userTask id="AActivity_BloodCollection" name="采集血样" />
    <bpmn:serviceTask id="AActivity_LabTesting" name="实验室检测" />
    <bpmn:exclusiveGateway id="AGateway_EvaluateResults" name="结果是否异常" />
    <bpmn:userTask id="AActivity_ReportAbnormal" name="生成异常报告" />
    <bpmn:userTask id="AActivity_ReportNormal" name="生成正常报告" />
    <bpmn:endEvent id="AEndEvent_Complete" name="检查完成" />
    <bpmn:endEvent id="AEndEvent_AbnormalFollowUp" name="需进一步检查" />
    <bpmn:sequenceFlow id="Flow_AStartEvent_AActivity_PatientRegistration" sourceRef="AStartEvent" targetRef="AActivity_PatientRegistration" />
    <bpmn:sequenceFlow id="Flow_AActivity_PatientRegistration_AActivity_BloodCollection" sourceRef="AActivity_PatientRegistration" targetRef="AActivity_BloodCollection" />
    <bpmn:sequenceFlow id="Flow_AActivity_BloodCollection_AActivity_LabTesting" sourceRef="AActivity_BloodCollection" targetRef="AActivity_LabTesting" />
    <bpmn:sequenceFlow id="Flow_AActivity_LabTesting_AGateway_EvaluateResults" sourceRef="AActivity_LabTesting" targetRef="AGateway_EvaluateResults" />
    <bpmn:sequenceFlow id="Flow_AGateway_EvaluateResults_AActivity_ReportNormal" sourceRef="AGateway_EvaluateResults" targetRef="AActivity_ReportNormal">
      <bpmn:conditionExpression>result == ''normal''</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:sequenceFlow id="Flow_AGateway_EvaluateResults_AActivity_ReportAbnormal" sourceRef="AGateway_EvaluateResults" targetRef="AActivity_ReportAbnormal">
      <bpmn:conditionExpression>result == ''abnormal''</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:sequenceFlow id="Flow_AActivity_ReportNormal_AEndEvent_Complete" sourceRef="AActivity_ReportNormal" targetRef="AEndEvent_Complete" />
    <bpmn:sequenceFlow id="Flow_AActivity_ReportAbnormal_AEndEvent_AbnormalFollowUp" sourceRef="AActivity_ReportAbnormal" targetRef="AEndEvent_AbnormalFollowUp" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="P001_uvqy">
      <bpmndi:BPMNShape id="Shape_AStartEvent" bpmnElement="AStartEvent">
        <dc:Bounds x="150" y="162" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AActivity_PatientRegistration" bpmnElement="AActivity_PatientRegistration">
        <dc:Bounds x="350" y="140" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AActivity_BloodCollection" bpmnElement="AActivity_BloodCollection">
        <dc:Bounds x="550" y="140" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AActivity_LabTesting" bpmnElement="AActivity_LabTesting">
        <dc:Bounds x="750" y="140" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AGateway_EvaluateResults" bpmnElement="AGateway_EvaluateResults" isMarkerVisible="true">
        <dc:Bounds x="950" y="162" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AActivity_ReportAbnormal" bpmnElement="AActivity_ReportAbnormal">
        <dc:Bounds x="1150" y="140" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AActivity_ReportNormal" bpmnElement="AActivity_ReportNormal">
        <dc:Bounds x="1150" y="301" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AEndEvent_Complete" bpmnElement="AEndEvent_Complete">
        <dc:Bounds x="1350" y="323" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AEndEvent_AbnormalFollowUp" bpmnElement="AEndEvent_AbnormalFollowUp">
        <dc:Bounds x="1350" y="162" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Edge_AStartEvent_AActivity_PatientRegistration" bpmnElement="Flow_AStartEvent_AActivity_PatientRegistration">
        <di:waypoint x="186" y="180" />
        <di:waypoint x="236" y="180" />
        <di:waypoint x="236" y="180" />
        <di:waypoint x="350" y="180" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AActivity_PatientRegistration_AActivity_BloodCollection" bpmnElement="Flow_AActivity_PatientRegistration_AActivity_BloodCollection">
        <di:waypoint x="450" y="180" />
        <di:waypoint x="500" y="180" />
        <di:waypoint x="550" y="180" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AActivity_BloodCollection_AActivity_LabTesting" bpmnElement="Flow_AActivity_BloodCollection_AActivity_LabTesting">
        <di:waypoint x="650" y="180" />
        <di:waypoint x="700" y="180" />
        <di:waypoint x="750" y="180" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AActivity_LabTesting_AGateway_EvaluateResults" bpmnElement="Flow_AActivity_LabTesting_AGateway_EvaluateResults">
        <di:waypoint x="850" y="180" />
        <di:waypoint x="900" y="180" />
        <di:waypoint x="900" y="180" />
        <di:waypoint x="950" y="180" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AGateway_EvaluateResults_AActivity_ReportNormal" bpmnElement="Flow_AGateway_EvaluateResults_AActivity_ReportNormal">
        <di:waypoint x="986" y="180" />
        <di:waypoint x="1036" y="180" />
        <di:waypoint x="1036" y="341" />
        <di:waypoint x="1150" y="341" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AGateway_EvaluateResults_AActivity_ReportAbnormal" bpmnElement="Flow_AGateway_EvaluateResults_AActivity_ReportAbnormal">
        <di:waypoint x="986" y="180" />
        <di:waypoint x="1036" y="180" />
        <di:waypoint x="1036" y="180" />
        <di:waypoint x="1150" y="180" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AActivity_ReportNormal_AEndEvent_Complete" bpmnElement="Flow_AActivity_ReportNormal_AEndEvent_Complete">
        <di:waypoint x="1250" y="341" />
        <di:waypoint x="1300" y="341" />
        <di:waypoint x="1300" y="341" />
        <di:waypoint x="1350" y="341" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AActivity_ReportAbnormal_AEndEvent_AbnormalFollowUp" bpmnElement="Flow_AActivity_ReportAbnormal_AEndEvent_AbnormalFollowUp">
        <di:waypoint x="1250" y="180" />
        <di:waypoint x="1300" y="180" />
        <di:waypoint x="1300" y="180" />
        <di:waypoint x="1350" y="180" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>', 0, NULL, 0, NULL, NULL, NULL, '2026-02-01 10:36:41.376373+08', '2026-02-01 10:36:41.376374+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (250, 'Process_1671', '2', 'Process_RAG智能客服', 'Process_Code_1671', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_1671" sf:code="Process_Code_1671" name="Process_RAG智能客服" isExecutable="true" sf:version="1">
    <bpmn:task id="Activity_1xm9cg4" name="接收客户的提问">
      <bpmn:extensionElements />
      <bpmn:incoming>Flow_0n48hom</bpmn:incoming>
      <bpmn:outgoing>Flow_18ev30d</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_0n48hom" sourceRef="StartEvent_1" targetRef="Activity_1xm9cg4" />
    <bpmn:endEvent id="Event_0jv1jk5" name="End">
      <bpmn:incoming>Flow_016alj5</bpmn:incoming>
    </bpmn:endEvent>
    <bpmn:sequenceFlow id="Flow_016alj5" sourceRef="Activity_0qbx206" targetRef="Event_0jv1jk5" />
    <bpmn:serviceTask id="Activity_0fqciiy" name="检索知识库, 并回答">
      <bpmn:extensionElements>
        <sf:aiServices>
          <sf:aiService type="RAG" />
        </sf:aiServices>
        <sf:sections>
          <sf:section name="myProperties">{"isNotifyClient":true}</sf:section>
        </sf:sections>
        <sf:performers />
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_1qbvvu0</bpmn:incoming>
      <bpmn:outgoing>Flow_1c5ulmv</bpmn:outgoing>
    </bpmn:serviceTask>
    <bpmn:startEvent id="StartEvent_1" name="Start">
      <bpmn:outgoing>Flow_0n48hom</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:serviceTask id="Activity_0qbx206" name="保存会话记录">
      <bpmn:extensionElements>
        <sf:services>
          <sf:service methodType="LocalService" argus="user_message, ai_response,customer" expression="Slickflow.Module.External.Customer.ConversationService" />
        </sf:services>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_1hjp392</bpmn:incoming>
      <bpmn:outgoing>Flow_016alj5</bpmn:outgoing>
    </bpmn:serviceTask>
    <bpmn:serviceTask id="Activity_1frdvp6" name="大模型提取客户联系方式">
      <bpmn:extensionElements>
        <sf:aiServices>
          <sf:aiService type="LLM" />
        </sf:aiServices>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_1c5ulmv</bpmn:incoming>
      <bpmn:outgoing>Flow_0byjux4</bpmn:outgoing>
    </bpmn:serviceTask>
    <bpmn:sequenceFlow id="Flow_1c5ulmv" sourceRef="Activity_0fqciiy" targetRef="Activity_1frdvp6" />
    <bpmn:sequenceFlow id="Flow_0byjux4" sourceRef="Activity_1frdvp6" targetRef="Activity_0lwjjqa" />
    <bpmn:sequenceFlow id="Flow_1hjp392" sourceRef="Activity_0lwjjqa" targetRef="Activity_0qbx206" />
    <bpmn:serviceTask id="Activity_0lwjjqa" name="解析并保存客户联系数据">
      <bpmn:extensionElements>
        <sf:services>
          <sf:service methodType="LocalService" argus="contact_json" expression="Slickflow.Module.External.Customer.CustomerContactService" />
        </sf:services>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_0byjux4</bpmn:incoming>
      <bpmn:outgoing>Flow_1hjp392</bpmn:outgoing>
    </bpmn:serviceTask>
    <bpmn:sequenceFlow id="Flow_18ev30d" sourceRef="Activity_1xm9cg4" targetRef="Activity_1944tqg" />
    <bpmn:sequenceFlow id="Flow_06wvawk" sourceRef="Activity_1944tqg" targetRef="Activity_05et6e4" />
    <bpmn:sequenceFlow id="Flow_1qbvvu0" sourceRef="Activity_05et6e4" targetRef="Activity_0fqciiy" />
    <bpmn:serviceTask id="Activity_1944tqg" name="查询客户记录">
      <bpmn:extensionElements>
        <sf:services>
          <sf:service methodType="LocalService" argus="customer_id" expression="Slickflow.Module.External.Customer.CustomerLoadService" />
        </sf:services>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_18ev30d</bpmn:incoming>
      <bpmn:outgoing>Flow_06wvawk</bpmn:outgoing>
    </bpmn:serviceTask>
    <bpmn:serviceTask id="Activity_05et6e4" name="查询会话历史记录">
      <bpmn:extensionElements>
        <sf:services>
          <sf:service methodType="LocalService" argus="customer_id,session_id" expression="Slickflow.Module.External.Customer.ConversationHistoryService" />
        </sf:services>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_06wvawk</bpmn:incoming>
      <bpmn:outgoing>Flow_1qbvvu0</bpmn:outgoing>
    </bpmn:serviceTask>
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_1671">
      <bpmndi:BPMNShape id="Activity_1xm9cg4_di" bpmnElement="Activity_1xm9cg4">
        <dc:Bounds x="220" y="280" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0jv1jk5_di" bpmnElement="Event_0jv1jk5">
        <dc:Bounds x="1442" y="302" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1450" y="345" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0fqciiy_di" bpmnElement="Activity_0fqciiy">
        <dc:Bounds x="740" y="280" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0dv2tay_di" bpmnElement="StartEvent_1">
        <dc:Bounds x="122" y="302" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="128" y="338" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0eufl05_di" bpmnElement="Activity_0qbx206">
        <dc:Bounds x="1260" y="280" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1frdvp6_di" bpmnElement="Activity_1frdvp6">
        <dc:Bounds x="910" y="280" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0p94eah_di" bpmnElement="Activity_0lwjjqa">
        <dc:Bounds x="1090" y="280" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0cw8tku_di" bpmnElement="Activity_1944tqg">
        <dc:Bounds x="390" y="280" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0btmlmp_di" bpmnElement="Activity_05et6e4">
        <dc:Bounds x="560" y="280" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_0n48hom_di" bpmnElement="Flow_0n48hom">
        <di:waypoint x="158" y="320" />
        <di:waypoint x="220" y="320" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_016alj5_di" bpmnElement="Flow_016alj5">
        <di:waypoint x="1360" y="320" />
        <di:waypoint x="1442" y="320" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1c5ulmv_di" bpmnElement="Flow_1c5ulmv">
        <di:waypoint x="840" y="320" />
        <di:waypoint x="910" y="320" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0byjux4_di" bpmnElement="Flow_0byjux4">
        <di:waypoint x="1010" y="320" />
        <di:waypoint x="1090" y="320" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1hjp392_di" bpmnElement="Flow_1hjp392">
        <di:waypoint x="1190" y="320" />
        <di:waypoint x="1260" y="320" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_18ev30d_di" bpmnElement="Flow_18ev30d">
        <di:waypoint x="320" y="320" />
        <di:waypoint x="390" y="320" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_06wvawk_di" bpmnElement="Flow_06wvawk">
        <di:waypoint x="490" y="320" />
        <di:waypoint x="560" y="320" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1qbvvu0_di" bpmnElement="Flow_1qbvvu0">
        <di:waypoint x="660" y="320" />
        <di:waypoint x="740" y="320" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
', 0, NULL, 0, NULL, NULL, NULL, '2026-02-25 01:18:30.767712+08', '2026-06-01 09:01:57.252052+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (251, 'Process_ApprovalFlow_nljn', '1', '任务审批流', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf">
  <bpmn:process id="Process_ApprovalFlow_nljn" name="任务审批流" isExecutable="false">
    <bpmn:startEvent id="AStartEvent_Initiate" name="发起任务" />
    <bpmn:userTask id="AUserTask_Submit" name="提交审批">
      <bpmn:extensionElements>
        <sf:variables>
          <sf:variable name="days" type="Integer" defaultValue="" direction="Output" isReferenced="false" isRequired="true" />
        </sf:variables>
      </bpmn:extensionElements>
    </bpmn:userTask>
    <bpmn:exclusiveGateway id="AExclusiveGateway_ApprovalDecision" name="审批决策" />
    <bpmn:userTask id="AUserTask_Approve" name="审批通过" />
    <bpmn:userTask id="AUserTask_Reject" name="审批拒绝" />
    <bpmn:endEvent id="AEndEvent_Success" name="审批完成" />
    <bpmn:endEvent id="AEndEvent_Failure" name="审批终止" />
    <bpmn:sequenceFlow id="Flow_AStartEvent_Initiate_AUserTask_Submit" sourceRef="AStartEvent_Initiate" targetRef="AUserTask_Submit" />
    <bpmn:sequenceFlow id="Flow_AUserTask_Submit_AExclusiveGateway_ApprovalDecision" sourceRef="AUserTask_Submit" targetRef="AExclusiveGateway_ApprovalDecision" />
    <bpmn:sequenceFlow id="Flow_AExclusiveGateway_ApprovalDecision_AUserTask_Approve" name="days&#60;3" sourceRef="AExclusiveGateway_ApprovalDecision" targetRef="AUserTask_Approve">
      <bpmn:conditionExpression>(days &lt; 3)</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:sequenceFlow id="Flow_AExclusiveGateway_ApprovalDecision_AUserTask_Reject" name="days&#62;=3" sourceRef="AExclusiveGateway_ApprovalDecision" targetRef="AUserTask_Reject">
      <bpmn:conditionExpression>(days &gt;= 3)</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:sequenceFlow id="Flow_AUserTask_Approve_AEndEvent_Success" sourceRef="AUserTask_Approve" targetRef="AEndEvent_Success" />
    <bpmn:sequenceFlow id="Flow_AUserTask_Reject_AEndEvent_Failure" sourceRef="AUserTask_Reject" targetRef="AEndEvent_Failure" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_ApprovalFlow_nljn">
      <bpmndi:BPMNShape id="Shape_AStartEvent_Initiate" bpmnElement="AStartEvent_Initiate">
        <dc:Bounds x="150" y="162" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AUserTask_Submit" bpmnElement="AUserTask_Submit">
        <dc:Bounds x="350" y="140" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AExclusiveGateway_ApprovalDecision" bpmnElement="AExclusiveGateway_ApprovalDecision" isMarkerVisible="true">
        <dc:Bounds x="550" y="162" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AUserTask_Approve" bpmnElement="AUserTask_Approve">
        <dc:Bounds x="750" y="301" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AEndEvent_Success" bpmnElement="AEndEvent_Success">
        <dc:Bounds x="950" y="323" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AEndEvent_Failure" bpmnElement="AEndEvent_Failure">
        <dc:Bounds x="950" y="162" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_AUserTask_Reject" bpmnElement="AUserTask_Reject">
        <dc:Bounds x="750" y="140" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Edge_AStartEvent_Initiate_AUserTask_Submit" bpmnElement="Flow_AStartEvent_Initiate_AUserTask_Submit">
        <di:waypoint x="186" y="180" />
        <di:waypoint x="236" y="180" />
        <di:waypoint x="236" y="180" />
        <di:waypoint x="350" y="180" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AUserTask_Submit_AExclusiveGateway_ApprovalDecision" bpmnElement="Flow_AUserTask_Submit_AExclusiveGateway_ApprovalDecision">
        <di:waypoint x="450" y="180" />
        <di:waypoint x="500" y="180" />
        <di:waypoint x="500" y="180" />
        <di:waypoint x="550" y="180" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AExclusiveGateway_ApprovalDecision_AUserTask_Approve" bpmnElement="Flow_AExclusiveGateway_ApprovalDecision_AUserTask_Approve">
        <di:waypoint x="586" y="180" />
        <di:waypoint x="670" y="180" />
        <di:waypoint x="670" y="341" />
        <di:waypoint x="750" y="341" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="667" y="258" width="36" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AExclusiveGateway_ApprovalDecision_AUserTask_Reject" bpmnElement="Flow_AExclusiveGateway_ApprovalDecision_AUserTask_Reject">
        <di:waypoint x="586" y="180" />
        <di:waypoint x="750" y="180" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="688" y="163" width="43" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AUserTask_Approve_AEndEvent_Success" bpmnElement="Flow_AUserTask_Approve_AEndEvent_Success">
        <di:waypoint x="850" y="341" />
        <di:waypoint x="900" y="341" />
        <di:waypoint x="900" y="341" />
        <di:waypoint x="950" y="341" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_AUserTask_Reject_AEndEvent_Failure" bpmnElement="Flow_AUserTask_Reject_AEndEvent_Failure">
        <di:waypoint x="850" y="180" />
        <di:waypoint x="950" y="180" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
', 0, NULL, 0, NULL, NULL, NULL, '2026-03-03 15:10:24.381469+08', '2026-03-02 15:28:53.891015+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (252, 'process_8381', '1', 'ConditionalProcess_5520', 'ConditionalProcess_Code_5520', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?><bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd"><bpmn:process id="process_8381" name="ConditionalProcess_5520" isExecutable="true" sf:code="ConditionalProcess_Code_5520" sf:version="1"><bpmn:startEvent id="StartNode_3180" name="Start" sf:code="Activity_Start_o13h"><bpmn:outgoing>Flow_7770</bpmn:outgoing></bpmn:startEvent><bpmn:task id="TaskNode_4795" name="Reimbursement Submit" sf:code="task001"><bpmn:incoming>Flow_7770</bpmn:incoming><bpmn:outgoing>Flow_9860</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_1837" name="Finalcial Approval" sf:code="task002"><bpmn:incoming>Flow_9860</bpmn:incoming><bpmn:outgoing>Flow_6677</bpmn:outgoing></bpmn:task><bpmn:exclusiveGateway id="GatewayNode_5010" name="XOr-Split" sf:code="orsplit001"><bpmn:incoming>Flow_6677</bpmn:incoming><bpmn:outgoing>Flow_8344</bpmn:outgoing><bpmn:outgoing>Flow_8075</bpmn:outgoing></bpmn:exclusiveGateway><bpmn:task id="TaskNode_9382" name="Approved by the Director in Charge" sf:code="task010"><bpmn:incoming>Flow_8344</bpmn:incoming><bpmn:outgoing>Flow_9811</bpmn:outgoing></bpmn:task><bpmn:task id="TaskNode_4460" name="CEO Approval" sf:code="task020"><bpmn:incoming>Flow_8075</bpmn:incoming><bpmn:outgoing>Flow_7965</bpmn:outgoing></bpmn:task><bpmn:exclusiveGateway id="GatewayNode_3018" name="XOr-Join" sf:code="orjoin001"><bpmn:incoming>Flow_7965</bpmn:incoming><bpmn:incoming>Flow_9811</bpmn:incoming><bpmn:outgoing>Flow_2236</bpmn:outgoing></bpmn:exclusiveGateway><bpmn:endEvent id="EndNode_6802" name="end" sf:code="Activity_End_qu7f"><bpmn:incoming>Flow_2236</bpmn:incoming></bpmn:endEvent><bpmn:sequenceFlow id="Flow_7770" name="" sourceRef="StartNode_3180" targetRef="TaskNode_4795" /><bpmn:sequenceFlow id="Flow_9860" name="" sourceRef="TaskNode_4795" targetRef="TaskNode_1837" /><bpmn:sequenceFlow id="Flow_6677" name="" sourceRef="TaskNode_1837" targetRef="GatewayNode_5010" /><bpmn:sequenceFlow id="Flow_8344" name="money&lt;10000" sourceRef="GatewayNode_5010" targetRef="TaskNode_9382"><bpmn:conditionExpression>money&lt;10000</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_8075" name="money&gt;=10000" sourceRef="GatewayNode_5010" targetRef="TaskNode_4460"><bpmn:conditionExpression>money&gt;=10000</bpmn:conditionExpression></bpmn:sequenceFlow><bpmn:sequenceFlow id="Flow_7965" name="" sourceRef="TaskNode_4460" targetRef="GatewayNode_3018" /><bpmn:sequenceFlow id="Flow_9811" name="" sourceRef="TaskNode_9382" targetRef="GatewayNode_3018" /><bpmn:sequenceFlow id="Flow_2236" name="" sourceRef="GatewayNode_3018" targetRef="EndNode_6802" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="process_8381"><bpmndi:BPMNShape id="StartNode_3180_di" bpmnElement="StartNode_3180"><dc:Bounds x="240" y="180" width="36" height="36" /><bpmndi:BPMNLabel /></bpmndi:BPMNShape><bpmndi:BPMNShape id="TaskNode_4795_di" bpmnElement="TaskNode_4795"><dc:Bounds x="356" y="158" width="100" height="80" /><bpmndi:BPMNLabel /></bpmndi:BPMNShape><bpmndi:BPMNShape id="TaskNode_1837_di" bpmnElement="TaskNode_1837"><dc:Bounds x="536" y="158" width="100" height="80" /><bpmndi:BPMNLabel /></bpmndi:BPMNShape><bpmndi:BPMNShape id="GatewayNode_5010_di" bpmnElement="GatewayNode_5010"><dc:Bounds x="716" y="180" width="36" height="36" /><bpmndi:BPMNLabel /></bpmndi:BPMNShape><bpmndi:BPMNShape id="TaskNode_9382_di" bpmnElement="TaskNode_9382"><dc:Bounds x="832" y="230" width="100" height="80" /><bpmndi:BPMNLabel /></bpmndi:BPMNShape><bpmndi:BPMNShape id="TaskNode_4460_di" bpmnElement="TaskNode_4460"><dc:Bounds x="832" y="70" width="100" height="80" /><bpmndi:BPMNLabel /></bpmndi:BPMNShape><bpmndi:BPMNShape id="GatewayNode_3018_di" bpmnElement="GatewayNode_3018"><dc:Bounds x="1012" y="180" width="36" height="36" /><bpmndi:BPMNLabel /></bpmndi:BPMNShape><bpmndi:BPMNShape id="EndNode_6802_di" bpmnElement="EndNode_6802"><dc:Bounds x="1128" y="180" width="36" height="36" /><bpmndi:BPMNLabel /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Flow_7770_di" bpmnElement="Flow_7770"><di:waypoint x="276" y="198" /><di:waypoint x="356" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_9860_di" bpmnElement="Flow_9860"><di:waypoint x="456" y="198" /><di:waypoint x="536" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_6677_di" bpmnElement="Flow_6677"><di:waypoint x="636" y="198" /><di:waypoint x="716" y="198" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_8344_di" bpmnElement="Flow_8344"><di:waypoint x="734" y="216" /><di:waypoint x="734" y="270" /><di:waypoint x="832" y="270" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_8075_di" bpmnElement="Flow_8075"><di:waypoint x="734" y="180" /><di:waypoint x="734" y="110" /><di:waypoint x="832" y="110" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_7965_di" bpmnElement="Flow_7965"><di:waypoint x="932" y="110" /><di:waypoint x="1030" y="110" /><di:waypoint x="1030" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_9811_di" bpmnElement="Flow_9811"><di:waypoint x="932" y="270" /><di:waypoint x="1030" y="270" /><di:waypoint x="1030" y="216" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Flow_2236_di" bpmnElement="Flow_2236"><di:waypoint x="1048" y="198" /><di:waypoint x="1128" y="198" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, 0, NULL, NULL, NULL, '2026-03-03 09:50:21.873463+08', '2026-03-03 09:50:21.873464+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (258, 'Process_1671', '3', 'Process_RAG智能客服_English', 'Process_Code_1671', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_1671" sf:code="Process_Code_1671" name="Process_RAG智能客服_English" isExecutable="true" sf:version="1">
    <bpmn:task id="Activity_1xm9cg4" name="接收客户的提问">
      <bpmn:extensionElements>
        <sf:variables />
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_0n48hom</bpmn:incoming>
      <bpmn:outgoing>Flow_18ev30d</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_0n48hom" sourceRef="StartEvent_1" targetRef="Activity_1xm9cg4" />
    <bpmn:endEvent id="Event_0jv1jk5" name="End">
      <bpmn:incoming>Flow_016alj5</bpmn:incoming>
    </bpmn:endEvent>
    <bpmn:sequenceFlow id="Flow_016alj5" sourceRef="Activity_0qbx206" targetRef="Event_0jv1jk5" />
    <bpmn:serviceTask id="Activity_0fqciiy" name="检索知识库, 并回答">
      <bpmn:extensionElements>
        <sf:aiServices>
          <sf:aiService type="RAG" />
        </sf:aiServices>
        <sf:variables>
          <sf:variable name="ai_response" type="String" defaultValue="" direction="Output" isReferenced="false" isRequired="true" />
        </sf:variables>
        <sf:sections>
          <sf:section name="myProperties">{"isNotifyClient":true}</sf:section>
        </sf:sections>
        <sf:performers />
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_1qbvvu0</bpmn:incoming>
      <bpmn:outgoing>Flow_1c5ulmv</bpmn:outgoing>
    </bpmn:serviceTask>
    <bpmn:startEvent id="StartEvent_1" name="Start">
      <bpmn:outgoing>Flow_0n48hom</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:serviceTask id="Activity_0qbx206" name="保存会话记录">
      <bpmn:extensionElements>
        <sf:variables />
        <sf:services>
          <sf:service methodType="LocalService" argus="user_message, ai_response,customer" expression="Slickflow.Module.External.Customer.ConversationService" />
        </sf:services>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_1hjp392</bpmn:incoming>
      <bpmn:outgoing>Flow_016alj5</bpmn:outgoing>
    </bpmn:serviceTask>
    <bpmn:serviceTask id="Activity_1frdvp6" name="大模型提取客户联系方式">
      <bpmn:extensionElements>
        <sf:aiServices>
          <sf:aiService type="LLM" />
        </sf:aiServices>
        <sf:variables>
          <sf:variable name="contact_json" type="String" defaultValue="" direction="Output" isReferenced="false" isRequired="true" />
        </sf:variables>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_1c5ulmv</bpmn:incoming>
      <bpmn:outgoing>Flow_0byjux4</bpmn:outgoing>
    </bpmn:serviceTask>
    <bpmn:sequenceFlow id="Flow_1c5ulmv" sourceRef="Activity_0fqciiy" targetRef="Activity_1frdvp6" />
    <bpmn:sequenceFlow id="Flow_0byjux4" sourceRef="Activity_1frdvp6" targetRef="Activity_0lwjjqa" />
    <bpmn:sequenceFlow id="Flow_1hjp392" sourceRef="Activity_0lwjjqa" targetRef="Activity_0qbx206" />
    <bpmn:serviceTask id="Activity_0lwjjqa" name="解析并保存客户联系数据">
      <bpmn:extensionElements>
        <sf:variables />
        <sf:services>
          <sf:service methodType="LocalService" argus="contact_json" expression="Slickflow.Module.External.Customer.CustomerContactService" />
        </sf:services>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_0byjux4</bpmn:incoming>
      <bpmn:outgoing>Flow_1hjp392</bpmn:outgoing>
    </bpmn:serviceTask>
    <bpmn:sequenceFlow id="Flow_18ev30d" sourceRef="Activity_1xm9cg4" targetRef="Activity_1944tqg" />
    <bpmn:sequenceFlow id="Flow_06wvawk" sourceRef="Activity_1944tqg" targetRef="Activity_05et6e4" />
    <bpmn:sequenceFlow id="Flow_1qbvvu0" sourceRef="Activity_05et6e4" targetRef="Activity_0fqciiy" />
    <bpmn:serviceTask id="Activity_1944tqg" name="查询客户记录">
      <bpmn:extensionElements>
        <sf:services>
          <sf:service methodType="LocalService" argus="customer_id" expression="Slickflow.Module.External.Customer.CustomerLoadService" />
        </sf:services>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_18ev30d</bpmn:incoming>
      <bpmn:outgoing>Flow_06wvawk</bpmn:outgoing>
    </bpmn:serviceTask>
    <bpmn:serviceTask id="Activity_05et6e4" name="查询会话历史记录">
      <bpmn:extensionElements>
        <sf:services>
          <sf:service methodType="LocalService" argus="customer_id,session_id" expression="Slickflow.Module.External.Customer.ConversationHistoryService" />
        </sf:services>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_06wvawk</bpmn:incoming>
      <bpmn:outgoing>Flow_1qbvvu0</bpmn:outgoing>
    </bpmn:serviceTask>
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_1671">
      <bpmndi:BPMNShape id="Activity_1xm9cg4_di" bpmnElement="Activity_1xm9cg4">
        <dc:Bounds x="220" y="280" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0jv1jk5_di" bpmnElement="Event_0jv1jk5">
        <dc:Bounds x="1442" y="302" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1450" y="345" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0fqciiy_di" bpmnElement="Activity_0fqciiy">
        <dc:Bounds x="740" y="280" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_0dv2tay_di" bpmnElement="StartEvent_1">
        <dc:Bounds x="122" y="302" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="128" y="338" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0eufl05_di" bpmnElement="Activity_0qbx206">
        <dc:Bounds x="1260" y="280" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1frdvp6_di" bpmnElement="Activity_1frdvp6">
        <dc:Bounds x="910" y="280" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0p94eah_di" bpmnElement="Activity_0lwjjqa">
        <dc:Bounds x="1090" y="280" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0cw8tku_di" bpmnElement="Activity_1944tqg">
        <dc:Bounds x="390" y="280" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0btmlmp_di" bpmnElement="Activity_05et6e4">
        <dc:Bounds x="560" y="280" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_0n48hom_di" bpmnElement="Flow_0n48hom">
        <di:waypoint x="158" y="320" />
        <di:waypoint x="220" y="320" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_016alj5_di" bpmnElement="Flow_016alj5">
        <di:waypoint x="1360" y="320" />
        <di:waypoint x="1442" y="320" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1c5ulmv_di" bpmnElement="Flow_1c5ulmv">
        <di:waypoint x="840" y="320" />
        <di:waypoint x="910" y="320" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0byjux4_di" bpmnElement="Flow_0byjux4">
        <di:waypoint x="1010" y="320" />
        <di:waypoint x="1090" y="320" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1hjp392_di" bpmnElement="Flow_1hjp392">
        <di:waypoint x="1190" y="320" />
        <di:waypoint x="1260" y="320" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_18ev30d_di" bpmnElement="Flow_18ev30d">
        <di:waypoint x="320" y="320" />
        <di:waypoint x="390" y="320" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_06wvawk_di" bpmnElement="Flow_06wvawk">
        <di:waypoint x="490" y="320" />
        <di:waypoint x="560" y="320" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1qbvvu0_di" bpmnElement="Flow_1qbvvu0">
        <di:waypoint x="660" y="320" />
        <di:waypoint x="740" y="320" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
', 0, NULL, 0, NULL, NULL, NULL, '2026-03-05 20:44:25.199933+08', '2026-03-04 21:13:01.243461+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (259, 'Process_2067', '1', 'Process_LeaveApproval_RuleEngine_2067', 'Process_Code_2067', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_2067" sf:code="Process_Code_2067" name="Process_LeaveApproval_RuleEngine_2067" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="StartEvent_1" name="Start">
      <bpmn:outgoing>Flow_155b1vf</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:task id="Activity_0dj7qf5" name="Submit Leave Request">
      <bpmn:incoming>Flow_155b1vf</bpmn:incoming>
      <bpmn:outgoing>Flow_0whst62</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_155b1vf" sourceRef="StartEvent_1" targetRef="Activity_0dj7qf5" />
    <bpmn:sequenceFlow id="Flow_0whst62" sourceRef="Activity_0dj7qf5" targetRef="Activity_0r22r3d" />
    <bpmn:businessRuleTask id="Activity_0r22r3d" name="Rule Validate">
      <bpmn:extensionElements>
        <sf:ruleConfigs>
          <sf:ruleConfig ruleSetCode="Leave_Approval_Dsl" />
        </sf:ruleConfigs>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_0whst62</bpmn:incoming>
      <bpmn:outgoing>Flow_1ah5746</bpmn:outgoing>
    </bpmn:businessRuleTask>
    <bpmn:task id="Activity_0hcj6a7" name="HR Approval">
      <bpmn:incoming>Flow_087u4tw</bpmn:incoming>
      <bpmn:outgoing>Flow_1wxvnki</bpmn:outgoing>
    </bpmn:task>
    <bpmn:endEvent id="Event_14r174j" name="End">
      <bpmn:incoming>Flow_1wxvnki</bpmn:incoming>
      <bpmn:incoming>Flow_1fdvamu</bpmn:incoming>
      <bpmn:incoming>Flow_1i1akzd</bpmn:incoming>
    </bpmn:endEvent>
    <bpmn:sequenceFlow id="Flow_1wxvnki" sourceRef="Activity_0hcj6a7" targetRef="Event_14r174j" />
    <bpmn:exclusiveGateway id="Gateway_1t2mh7h">
      <bpmn:incoming>Flow_1ah5746</bpmn:incoming>
      <bpmn:outgoing>Flow_087u4tw</bpmn:outgoing>
      <bpmn:outgoing>Flow_01hsi9e</bpmn:outgoing>
      <bpmn:outgoing>Flow_1wrt5qd</bpmn:outgoing>
    </bpmn:exclusiveGateway>
    <bpmn:sequenceFlow id="Flow_1ah5746" sourceRef="Activity_0r22r3d" targetRef="Gateway_1t2mh7h" />
    <bpmn:sequenceFlow id="Flow_087u4tw" name="ApprovalLevel==&#34;HR&#34;" sourceRef="Gateway_1t2mh7h" targetRef="Activity_0hcj6a7">
      <bpmn:conditionExpression>(ApprovalLevel == "HR")</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:task id="Activity_1imqlwu" name="Manager Approval">
      <bpmn:incoming>Flow_01hsi9e</bpmn:incoming>
      <bpmn:outgoing>Flow_1fdvamu</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_01hsi9e" name="ApprovalLevel-&#34;Manager&#34;" sourceRef="Gateway_1t2mh7h" targetRef="Activity_1imqlwu">
      <bpmn:conditionExpression>(ApprovalLevel == "Manager")</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:task id="Activity_0cqcuhr" name="Leader Approval">
      <bpmn:incoming>Flow_1wrt5qd</bpmn:incoming>
      <bpmn:outgoing>Flow_1i1akzd</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_1wrt5qd" name="ApprovalLevel==&#34;Leader&#34;" sourceRef="Gateway_1t2mh7h" targetRef="Activity_0cqcuhr">
      <bpmn:conditionExpression>(ApprovalLevel == "Leader")</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:sequenceFlow id="Flow_1fdvamu" sourceRef="Activity_1imqlwu" targetRef="Event_14r174j" />
    <bpmn:sequenceFlow id="Flow_1i1akzd" sourceRef="Activity_0cqcuhr" targetRef="Event_14r174j" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_2067">
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="342" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="348" y="276" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0dj7qf5_di" bpmnElement="Activity_0dj7qf5">
        <dc:Bounds x="500" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_11enxp6_di" bpmnElement="Activity_0r22r3d">
        <dc:Bounds x="720" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0hcj6a7_di" bpmnElement="Activity_0hcj6a7">
        <dc:Bounds x="1060" y="30" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Event_14r174j_di" bpmnElement="Event_14r174j">
        <dc:Bounds x="1372" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1380" y="283" width="20" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_1t2mh7h_di" bpmnElement="Gateway_1t2mh7h" isMarkerVisible="true">
        <dc:Bounds x="945" y="233" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1imqlwu_di" bpmnElement="Activity_1imqlwu">
        <dc:Bounds x="1060" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0cqcuhr_di" bpmnElement="Activity_0cqcuhr">
        <dc:Bounds x="1070" y="390" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_155b1vf_di" bpmnElement="Flow_155b1vf">
        <di:waypoint x="378" y="258" />
        <di:waypoint x="500" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0whst62_di" bpmnElement="Flow_0whst62">
        <di:waypoint x="600" y="258" />
        <di:waypoint x="720" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1wxvnki_di" bpmnElement="Flow_1wxvnki">
        <di:waypoint x="1160" y="70" />
        <di:waypoint x="1280" y="70" />
        <di:waypoint x="1280" y="258" />
        <di:waypoint x="1372" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1ah5746_di" bpmnElement="Flow_1ah5746">
        <di:waypoint x="820" y="258" />
        <di:waypoint x="945" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_087u4tw_di" bpmnElement="Flow_087u4tw">
        <di:waypoint x="970" y="233" />
        <di:waypoint x="970" y="70" />
        <di:waypoint x="1060" y="70" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="944" y="149" width="83" height="27" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_01hsi9e_di" bpmnElement="Flow_01hsi9e">
        <di:waypoint x="995" y="258" />
        <di:waypoint x="1060" y="258" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="991" y="240" width="74" height="27" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1wrt5qd_di" bpmnElement="Flow_1wrt5qd">
        <di:waypoint x="970" y="283" />
        <di:waypoint x="970" y="430" />
        <di:waypoint x="1070" y="430" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="942" y="354" width="87" height="27" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1fdvamu_di" bpmnElement="Flow_1fdvamu">
        <di:waypoint x="1160" y="258" />
        <di:waypoint x="1372" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1i1akzd_di" bpmnElement="Flow_1i1akzd">
        <di:waypoint x="1170" y="430" />
        <di:waypoint x="1280" y="430" />
        <di:waypoint x="1280" y="258" />
        <di:waypoint x="1372" y="258" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
', 0, NULL, 0, NULL, NULL, NULL, '2026-03-17 00:57:13.036701+08', '2026-04-08 11:45:17.789951+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (260, 'Process_9980', '1', 'Process_Auto_Loop_Order', 'Process_Code_9980', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_9980" sf:code="Process_Code_9980" name="Process_Auto_Loop_Order" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="StartEvent_1" name="Start">
      <bpmn:outgoing>Flow_0o5tvb0</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:task id="Activity_1ccvzyp" name="下一轮挂单">
      <bpmn:incoming>Flow_0l4ymv0</bpmn:incoming>
      <bpmn:incoming>Flow_193sm2f</bpmn:incoming>
      <bpmn:incoming>Flow_1ylr5pu</bpmn:incoming>
      <bpmn:incoming>Flow_03h9fmo</bpmn:incoming>
      <bpmn:outgoing>Flow_0at71u8</bpmn:outgoing>
    </bpmn:task>
    <bpmn:exclusiveGateway id="Gateway_1rh37w7" name="是否撮合成功？">
      <bpmn:incoming>Flow_04ae3bm</bpmn:incoming>
      <bpmn:outgoing>Flow_1khshom</bpmn:outgoing>
      <bpmn:outgoing>Flow_0kpc22n</bpmn:outgoing>
    </bpmn:exclusiveGateway>
    <bpmn:task id="Activity_078q8z7" name="进入下一轮交易">
      <bpmn:incoming>Flow_1khshom</bpmn:incoming>
      <bpmn:incoming>Flow_0po3dtt</bpmn:incoming>
      <bpmn:outgoing>Flow_13uljwq</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_1khshom" name="Yes" sourceRef="Gateway_1rh37w7" targetRef="Activity_078q8z7" />
    <bpmn:exclusiveGateway id="Gateway_0vm1g7l" name="交易判定为赢？">
      <bpmn:incoming>Flow_0ccj6hd</bpmn:incoming>
      <bpmn:outgoing>Flow_08xt1hi</bpmn:outgoing>
      <bpmn:outgoing>Flow_193sm2f</bpmn:outgoing>
    </bpmn:exclusiveGateway>
    <bpmn:sequenceFlow id="Flow_13uljwq" sourceRef="Activity_078q8z7" targetRef="Activity_0dpqtyx" />
    <bpmn:task id="Activity_056ncsy" name="发起下一轮的吃单">
      <bpmn:incoming>Flow_08xt1hi</bpmn:incoming>
      <bpmn:outgoing>Flow_0og8dob</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_08xt1hi" name="Yes" sourceRef="Gateway_0vm1g7l" targetRef="Activity_056ncsy" />
    <bpmn:exclusiveGateway id="Gateway_0j6l89d" name="成功吃单？">
      <bpmn:incoming>Flow_0og8dob</bpmn:incoming>
      <bpmn:outgoing>Flow_0po3dtt</bpmn:outgoing>
      <bpmn:outgoing>Flow_0l4ymv0</bpmn:outgoing>
    </bpmn:exclusiveGateway>
    <bpmn:sequenceFlow id="Flow_0og8dob" sourceRef="Activity_056ncsy" targetRef="Gateway_0j6l89d" />
    <bpmn:sequenceFlow id="Flow_0po3dtt" name="Yes" sourceRef="Gateway_0j6l89d" targetRef="Activity_078q8z7" />
    <bpmn:sequenceFlow id="Flow_0l4ymv0" name="No" sourceRef="Gateway_0j6l89d" targetRef="Activity_1ccvzyp" />
    <bpmn:sequenceFlow id="Flow_193sm2f" name="No" sourceRef="Gateway_0vm1g7l" targetRef="Activity_1ccvzyp" />
    <bpmn:task id="Activity_0hzt5vw" name="撤单">
      <bpmn:incoming>Flow_0kpc22n</bpmn:incoming>
      <bpmn:outgoing>Flow_1ylr5pu</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_0kpc22n" name="No" sourceRef="Gateway_1rh37w7" targetRef="Activity_0hzt5vw" />
    <bpmn:sequenceFlow id="Flow_1ylr5pu" sourceRef="Activity_0hzt5vw" targetRef="Activity_1ccvzyp" />
    <bpmn:task id="Activity_12qujil" name="取当前时间">
      <bpmn:incoming>Flow_0o5tvb0</bpmn:incoming>
      <bpmn:outgoing>Flow_03h9fmo</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_0o5tvb0" sourceRef="StartEvent_1" targetRef="Activity_12qujil" />
    <bpmn:sequenceFlow id="Flow_03h9fmo" sourceRef="Activity_12qujil" targetRef="Activity_1ccvzyp" />
    <bpmn:task id="Activity_0dpqtyx" name="倒计时等待">
      <bpmn:incoming>Flow_13uljwq</bpmn:incoming>
      <bpmn:outgoing>Flow_0ccj6hd</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_0ccj6hd" sourceRef="Activity_0dpqtyx" targetRef="Gateway_0vm1g7l" />
    <bpmn:task id="Activity_1jthyv5" name="挂单成功，等待撮合">
      <bpmn:incoming>Flow_0at71u8</bpmn:incoming>
      <bpmn:outgoing>Flow_04ae3bm</bpmn:outgoing>
    </bpmn:task>
    <bpmn:sequenceFlow id="Flow_0at71u8" sourceRef="Activity_1ccvzyp" targetRef="Activity_1jthyv5" />
    <bpmn:sequenceFlow id="Flow_04ae3bm" sourceRef="Activity_1jthyv5" targetRef="Gateway_1rh37w7" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_9980">
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="152" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="158" y="276" width="25" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1ccvzyp_di" bpmnElement="Activity_1ccvzyp">
        <dc:Bounds x="440" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_1rh37w7_di" bpmnElement="Gateway_1rh37w7" isMarkerVisible="true">
        <dc:Bounds x="775" y="233" width="50" height="50" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="761" y="293" width="77" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_078q8z7_di" bpmnElement="Activity_078q8z7">
        <dc:Bounds x="900" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_0vm1g7l_di" bpmnElement="Gateway_0vm1g7l" isMarkerVisible="true">
        <dc:Bounds x="1235" y="233" width="50" height="50" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1221" y="209" width="77" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_056ncsy_di" bpmnElement="Activity_056ncsy">
        <dc:Bounds x="1340" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Gateway_0j6l89d_di" bpmnElement="Gateway_0j6l89d" isMarkerVisible="true">
        <dc:Bounds x="1495" y="233" width="50" height="50" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1555" y="251" width="55" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0hzt5vw_di" bpmnElement="Activity_0hzt5vw">
        <dc:Bounds x="600" y="50" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_12qujil_di" bpmnElement="Activity_12qujil">
        <dc:Bounds x="260" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_0dpqtyx_di" bpmnElement="Activity_0dpqtyx">
        <dc:Bounds x="1070" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_1jthyv5_di" bpmnElement="Activity_1jthyv5">
        <dc:Bounds x="600" y="218" width="100" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_1khshom_di" bpmnElement="Flow_1khshom">
        <di:waypoint x="825" y="258" />
        <di:waypoint x="900" y="258" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="854" y="240" width="18" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_13uljwq_di" bpmnElement="Flow_13uljwq">
        <di:waypoint x="1000" y="258" />
        <di:waypoint x="1070" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_08xt1hi_di" bpmnElement="Flow_08xt1hi">
        <di:waypoint x="1285" y="258" />
        <di:waypoint x="1340" y="258" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1304" y="240" width="18" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0og8dob_di" bpmnElement="Flow_0og8dob">
        <di:waypoint x="1440" y="258" />
        <di:waypoint x="1495" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0po3dtt_di" bpmnElement="Flow_0po3dtt">
        <di:waypoint x="1520" y="233" />
        <di:waypoint x="1520" y="150" />
        <di:waypoint x="950" y="150" />
        <di:waypoint x="950" y="218" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1226" y="132" width="18" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0l4ymv0_di" bpmnElement="Flow_0l4ymv0">
        <di:waypoint x="1520" y="283" />
        <di:waypoint x="1520" y="450" />
        <di:waypoint x="490" y="450" />
        <di:waypoint x="490" y="298" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="998" y="432" width="15" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_193sm2f_di" bpmnElement="Flow_193sm2f">
        <di:waypoint x="1260" y="283" />
        <di:waypoint x="1260" y="450" />
        <di:waypoint x="490" y="450" />
        <di:waypoint x="490" y="298" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="868" y="432" width="15" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0kpc22n_di" bpmnElement="Flow_0kpc22n">
        <di:waypoint x="800" y="233" />
        <di:waypoint x="800" y="90" />
        <di:waypoint x="700" y="90" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="746" y="99" width="15" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_1ylr5pu_di" bpmnElement="Flow_1ylr5pu">
        <di:waypoint x="600" y="90" />
        <di:waypoint x="490" y="90" />
        <di:waypoint x="490" y="218" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0o5tvb0_di" bpmnElement="Flow_0o5tvb0">
        <di:waypoint x="188" y="258" />
        <di:waypoint x="260" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_03h9fmo_di" bpmnElement="Flow_03h9fmo">
        <di:waypoint x="360" y="258" />
        <di:waypoint x="440" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0ccj6hd_di" bpmnElement="Flow_0ccj6hd">
        <di:waypoint x="1170" y="258" />
        <di:waypoint x="1235" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_0at71u8_di" bpmnElement="Flow_0at71u8">
        <di:waypoint x="540" y="258" />
        <di:waypoint x="600" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_04ae3bm_di" bpmnElement="Flow_04ae3bm">
        <di:waypoint x="700" y="258" />
        <di:waypoint x="775" y="258" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>', 0, NULL, 0, NULL, NULL, NULL, '2026-04-26 10:40:14.21118+08', '2026-04-26 10:40:14.21118+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (261, 'Process_SmartProcurement_8529', '1', '智能采购多智能体工作流', 'SmartProcurement_8529', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd">
  <bpmn:process id="Process_SmartProcurement_8529" sf:code="SmartProcurement_8529" name="智能采购多智能体工作流" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="StartEvent_1" name="开始">
      <bpmn:outgoing>Flow_start_001</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:serviceTask id="Activity_agent001" sf:code="AGENT_001" name="需求分析">
      <bpmn:extensionElements>
        <sf:aiServices>
          <sf:aiService type="Agent" />
        </sf:aiServices>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_start_001</bpmn:incoming>
      <bpmn:outgoing>Flow_001_002</bpmn:outgoing>
    </bpmn:serviceTask>
    <bpmn:serviceTask id="Activity_agent002" sf:code="AGENT_002" name="供应商搜索">
      <bpmn:extensionElements>
        <sf:aiServices>
          <sf:aiService type="Agent" />
        </sf:aiServices>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_001_002</bpmn:incoming>
      <bpmn:outgoing>Flow_002_003</bpmn:outgoing>
    </bpmn:serviceTask>
    <bpmn:serviceTask id="Activity_agent003" sf:code="AGENT_003" name="合规审查">
      <bpmn:extensionElements>
        <sf:aiServices>
          <sf:aiService type="Agent" />
        </sf:aiServices>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_002_003</bpmn:incoming>
      <bpmn:outgoing>Flow_003_004</bpmn:outgoing>
    </bpmn:serviceTask>
    <bpmn:serviceTask id="Activity_agent004" sf:code="AGENT_004" name="价格评估">
      <bpmn:extensionElements>
        <sf:aiServices>
          <sf:aiService type="Agent" />
        </sf:aiServices>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_003_004</bpmn:incoming>
      <bpmn:outgoing>Flow_004_005</bpmn:outgoing>
    </bpmn:serviceTask>
    <bpmn:serviceTask id="Activity_agent005" sf:code="AGENT_005" name="下单执行">
      <bpmn:extensionElements>
        <sf:aiServices>
          <sf:aiService type="Agent" />
        </sf:aiServices>
      </bpmn:extensionElements>
      <bpmn:incoming>Flow_004_005</bpmn:incoming>
      <bpmn:outgoing>Flow_005_end</bpmn:outgoing>
    </bpmn:serviceTask>
    <bpmn:endEvent id="EndEvent_1" name="结束">
      <bpmn:incoming>Flow_005_end</bpmn:incoming>
    </bpmn:endEvent>
    <bpmn:sequenceFlow id="Flow_start_001" sourceRef="StartEvent_1" targetRef="Activity_agent001" />
    <bpmn:sequenceFlow id="Flow_001_002" sourceRef="Activity_agent001" targetRef="Activity_agent002" />
    <bpmn:sequenceFlow id="Flow_002_003" sourceRef="Activity_agent002" targetRef="Activity_agent003" />
    <bpmn:sequenceFlow id="Flow_003_004" sourceRef="Activity_agent003" targetRef="Activity_agent004" />
    <bpmn:sequenceFlow id="Flow_004_005" sourceRef="Activity_agent004" targetRef="Activity_agent005" />
    <bpmn:sequenceFlow id="Flow_005_end" sourceRef="Activity_agent005" targetRef="EndEvent_1" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_SmartProcurement_8529">
      <bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1">
        <dc:Bounds x="80" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="86" y="283" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_agent001_di" bpmnElement="Activity_agent001">
        <dc:Bounds x="158" y="218" width="140" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_agent002_di" bpmnElement="Activity_agent002">
        <dc:Bounds x="350" y="218" width="140" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_agent003_di" bpmnElement="Activity_agent003">
        <dc:Bounds x="542" y="218" width="140" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_agent004_di" bpmnElement="Activity_agent004">
        <dc:Bounds x="734" y="218" width="140" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Activity_agent005_di" bpmnElement="Activity_agent005">
        <dc:Bounds x="926" y="218" width="140" height="80" />
        <bpmndi:BPMNLabel />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="EndEvent_1_di" bpmnElement="EndEvent_1">
        <dc:Bounds x="1118" y="240" width="36" height="36" />
        <bpmndi:BPMNLabel>
          <dc:Bounds x="1124" y="283" width="24" height="14" />
        </bpmndi:BPMNLabel>
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Flow_start_001_di" bpmnElement="Flow_start_001">
        <di:waypoint x="116" y="258" />
        <di:waypoint x="158" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_001_002_di" bpmnElement="Flow_001_002">
        <di:waypoint x="298" y="258" />
        <di:waypoint x="350" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_002_003_di" bpmnElement="Flow_002_003">
        <di:waypoint x="490" y="258" />
        <di:waypoint x="542" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_003_004_di" bpmnElement="Flow_003_004">
        <di:waypoint x="682" y="258" />
        <di:waypoint x="734" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_004_005_di" bpmnElement="Flow_004_005">
        <di:waypoint x="874" y="258" />
        <di:waypoint x="926" y="258" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Flow_005_end_di" bpmnElement="Flow_005_end">
        <di:waypoint x="1066" y="258" />
        <di:waypoint x="1118" y="258" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
', 0, NULL, 0, NULL, NULL, '5-Agent ReAct 智能采购流程：需求分析→供应商搜索→合规审查→价格评估→下单执行，每个节点均为 Agent 类型 AI 服务任务，通过 AgentToolRegistry 注册业务工具驱动 ReAct 循环。', '2026-06-01 19:35:42.8832+08', '2026-06-01 09:17:13.337845+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (270, 'Process_ATGInquiry_2026', '1', 'ATG LED照明 AI询盘自动化流程', 'ATGInquiry_2026', 1, 'ATG', NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:sf="http://www.slickflow.com/schema/sf">
  <bpmn:process id="Process_ATGInquiry_2026" name="ATG LED照明 AI询盘自动化流程" isExecutable="true">
    <bpmn:startEvent id="StartEvent_1" name="接收询价表单" />
    <bpmn:serviceTask id="Activity_agent001" sf:code="AGENT_001" name="AI需求解析"><bpmn:extensionElements><sf:aiServices><sf:aiService type="Agent"/></sf:aiServices></bpmn:extensionElements></bpmn:serviceTask>
    <bpmn:serviceTask id="Activity_agent002" sf:code="AGENT_002" name="ERP产品匹配"><bpmn:extensionElements><sf:aiServices><sf:aiService type="Agent"/></sf:aiServices></bpmn:extensionElements></bpmn:serviceTask>
    <bpmn:serviceTask id="Activity_agent003" sf:code="AGENT_003" name="AI报价生成"><bpmn:extensionElements><sf:aiServices><sf:aiService type="Agent"/></sf:aiServices></bpmn:extensionElements></bpmn:serviceTask>
    <bpmn:userTask id="Activity_approval" name="销售审批" />
    <bpmn:exclusiveGateway id="Gateway_approval" name="审批结果" />
    <bpmn:serviceTask id="Activity_agent004" name="PDF生成+邮件发送">
      <bpmn:extensionElements>
        <sf:services>
          <sf:service methodType="LocalService" expression="Slickflow.Module.External.Document.PdfEmailService" />
        </sf:services>
      </bpmn:extensionElements>
    </bpmn:serviceTask>
    <bpmn:serviceTask id="Activity_agent005" sf:code="AGENT_005" name="重新生成报价"><bpmn:extensionElements><sf:aiServices><sf:aiService type="Agent"/></sf:aiServices></bpmn:extensionElements></bpmn:serviceTask>
    <bpmn:endEvent id="EndEvent_1" name="询盘处理完成" />
    <bpmn:sequenceFlow id="Flow_start_001" sourceRef="StartEvent_1" targetRef="Activity_agent001" />
    <bpmn:sequenceFlow id="Flow_001_002" sourceRef="Activity_agent001" targetRef="Activity_agent002" />
    <bpmn:sequenceFlow id="Flow_002_003" sourceRef="Activity_agent002" targetRef="Activity_agent003" />
    <bpmn:sequenceFlow id="Flow_003_004" sourceRef="Activity_agent003" targetRef="Activity_approval" />
    <bpmn:sequenceFlow id="Flow_004_gw" sourceRef="Activity_approval" targetRef="Gateway_approval" />
    <bpmn:sequenceFlow id="Flow_approved" name="通过" sourceRef="Gateway_approval" targetRef="Activity_agent004">
      <bpmn:conditionExpression>approved=="true"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:sequenceFlow id="Flow_rejected" name="驳回" sourceRef="Gateway_approval" targetRef="Activity_agent005">
      <bpmn:conditionExpression>approved=="false"</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:sequenceFlow id="Flow_back_approval" sourceRef="Activity_agent005" targetRef="Activity_approval" />
    <bpmn:sequenceFlow id="Flow_004_end" sourceRef="Activity_agent004" targetRef="EndEvent_1" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_1">
    <bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_ATGInquiry_2026">
      <bpmndi:BPMNShape id="Shape_StartEvent_1" bpmnElement="StartEvent_1">
        <dc:Bounds x="52" y="262" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_Activity_agent001" bpmnElement="Activity_agent001">
        <dc:Bounds x="150" y="240" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_Activity_agent002" bpmnElement="Activity_agent002">
        <dc:Bounds x="320" y="240" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_Activity_agent003" bpmnElement="Activity_agent003">
        <dc:Bounds x="490" y="240" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_Activity_approval" bpmnElement="Activity_approval">
        <dc:Bounds x="660" y="240" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_Gateway_approval" bpmnElement="Gateway_approval" isMarkerVisible="true">
        <dc:Bounds x="830" y="255" width="50" height="50" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_Activity_agent004" bpmnElement="Activity_agent004">
        <dc:Bounds x="950" y="160" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_Activity_agent005" bpmnElement="Activity_agent005">
        <dc:Bounds x="950" y="340" width="100" height="80" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_EndEvent_1" bpmnElement="EndEvent_1">
        <dc:Bounds x="1122" y="182" width="36" height="36" />
      </bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Edge_start_001" bpmnElement="Flow_start_001">
        <di:waypoint x="88" y="280" />
        <di:waypoint x="150" y="280" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_001_002" bpmnElement="Flow_001_002">
        <di:waypoint x="250" y="280" />
        <di:waypoint x="320" y="280" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_002_003" bpmnElement="Flow_002_003">
        <di:waypoint x="420" y="280" />
        <di:waypoint x="490" y="280" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_003_004" bpmnElement="Flow_003_004">
        <di:waypoint x="590" y="280" />
        <di:waypoint x="660" y="280" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_004_gw" bpmnElement="Flow_004_gw">
        <di:waypoint x="760" y="280" />
        <di:waypoint x="830" y="280" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_approved" bpmnElement="Flow_approved">
        <di:waypoint x="855" y="255" />
        <di:waypoint x="855" y="200" />
        <di:waypoint x="950" y="200" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_rejected" bpmnElement="Flow_rejected">
        <di:waypoint x="855" y="305" />
        <di:waypoint x="855" y="380" />
        <di:waypoint x="950" y="380" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_back_approval" bpmnElement="Flow_back_approval">
        <di:waypoint x="1000" y="420" />
        <di:waypoint x="1000" y="460" />
        <di:waypoint x="710" y="460" />
        <di:waypoint x="710" y="320" />
      </bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_004_end" bpmnElement="Flow_004_end">
        <di:waypoint x="1050" y="200" />
        <di:waypoint x="1122" y="200" />
      </bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>
', 0, NULL, 0, NULL, NULL, '5节点 Harness Multi-Agent 询盘流程：表单接收 → NeedsParser Agent(解析客户规格/认证需求) → ProductSelector Agent(ERP产品匹配+协议价查询) → Pricing Agent(AI报价生成,含节能ROI) → [UserTask]销售审批(Human-in-the-Loop) → Delivery Agent(PDF生成+邮件发送+CRM写入)。每个Agent节点均配置独立PromptTemplate和ToolSet。', '2026-06-03 07:21:47.829249+08', '2026-06-02 18:03:06.762557+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (272, 'Process_PurchaseOrder', '1', '采购申请', 'PurchaseOrder', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
    xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI"
    xmlns:dc="http://www.omg.org/spec/DD/20100524/DC"
    xmlns:di="http://www.omg.org/spec/DD/20100524/DI"
    xmlns:sf="http://www.slickflow.com/schema/sf"
    xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"
    id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn">
  <bpmn:process id="Process_PurchaseOrder" sf:code="PurchaseOrder" name="采购申请" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="Start_PO" sf:code="Start" name="开始">
      <bpmn:outgoing>Flow_PO1</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:task id="Task_PO1" sf:code="task001" name="提交采购申请">
      <bpmn:incoming>Flow_PO1</bpmn:incoming>
      <bpmn:outgoing>Flow_PO2</bpmn:outgoing>
    </bpmn:task>
    <bpmn:task id="Task_PO2" sf:code="task002" name="经理审批">
      <bpmn:incoming>Flow_PO2</bpmn:incoming>
      <bpmn:outgoing>Flow_PO3</bpmn:outgoing>
    </bpmn:task>
    <bpmn:endEvent id="End_PO" sf:code="End" name="结束">
      <bpmn:incoming>Flow_PO3</bpmn:incoming>
    </bpmn:endEvent>
    <bpmn:sequenceFlow id="Flow_PO1" sourceRef="Start_PO" targetRef="Task_PO1" />
    <bpmn:sequenceFlow id="Flow_PO2" sourceRef="Task_PO1" targetRef="Task_PO2" />
    <bpmn:sequenceFlow id="Flow_PO3" sourceRef="Task_PO2" targetRef="End_PO" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_PO">
    <bpmndi:BPMNPlane id="BPMNPlane_PO" bpmnElement="Process_PurchaseOrder">
      <bpmndi:BPMNShape id="Shape_Start_PO" bpmnElement="Start_PO"><dc:Bounds x="180" y="180" width="36" height="36"/></bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_Task_PO1" bpmnElement="Task_PO1"><dc:Bounds x="280" y="158" width="100" height="80"/></bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_Task_PO2" bpmnElement="Task_PO2"><dc:Bounds x="450" y="158" width="100" height="80"/></bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_End_PO" bpmnElement="End_PO"><dc:Bounds x="620" y="180" width="36" height="36"/></bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Edge_PO1" bpmnElement="Flow_PO1"><di:waypoint x="216" y="198"/><di:waypoint x="280" y="198"/></bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_PO2" bpmnElement="Flow_PO2"><di:waypoint x="380" y="198"/><di:waypoint x="450" y="198"/></bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_PO3" bpmnElement="Flow_PO3"><di:waypoint x="550" y="198"/><di:waypoint x="620" y="198"/></bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>', 0, NULL, 0, NULL, NULL, NULL, '2026-06-16 22:19:10.204845+08', '2026-06-16 22:19:10.204846+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (273, 'Process_ExpenseReport', '1', '报销申请', 'ExpenseReport', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
    xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI"
    xmlns:dc="http://www.omg.org/spec/DD/20100524/DC"
    xmlns:di="http://www.omg.org/spec/DD/20100524/DI"
    xmlns:sf="http://www.slickflow.com/schema/sf"
    xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"
    id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn">
  <bpmn:process id="Process_ExpenseReport" sf:code="ExpenseReport" name="报销申请" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="Start_ER" sf:code="Start" name="开始">
      <bpmn:outgoing>Flow_ER1</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:task id="Task_ER1" sf:code="task001" name="提交报销申请">
      <bpmn:incoming>Flow_ER1</bpmn:incoming>
      <bpmn:outgoing>Flow_ER2</bpmn:outgoing>
    </bpmn:task>
    <bpmn:task id="Task_ER2" sf:code="task002" name="财务审批">
      <bpmn:incoming>Flow_ER2</bpmn:incoming>
      <bpmn:outgoing>Flow_ER3</bpmn:outgoing>
    </bpmn:task>
    <bpmn:endEvent id="End_ER" sf:code="End" name="结束">
      <bpmn:incoming>Flow_ER3</bpmn:incoming>
    </bpmn:endEvent>
    <bpmn:sequenceFlow id="Flow_ER1" sourceRef="Start_ER" targetRef="Task_ER1" />
    <bpmn:sequenceFlow id="Flow_ER2" sourceRef="Task_ER1" targetRef="Task_ER2" />
    <bpmn:sequenceFlow id="Flow_ER3" sourceRef="Task_ER2" targetRef="End_ER" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_ER">
    <bpmndi:BPMNPlane id="BPMNPlane_ER" bpmnElement="Process_ExpenseReport">
      <bpmndi:BPMNShape id="Shape_Start_ER" bpmnElement="Start_ER"><dc:Bounds x="180" y="180" width="36" height="36"/></bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_Task_ER1" bpmnElement="Task_ER1"><dc:Bounds x="280" y="158" width="100" height="80"/></bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_Task_ER2" bpmnElement="Task_ER2"><dc:Bounds x="450" y="158" width="100" height="80"/></bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_End_ER" bpmnElement="End_ER"><dc:Bounds x="620" y="180" width="36" height="36"/></bpmndi:BPMNShape>
      <bpmndi:BPMNEdge id="Edge_ER1" bpmnElement="Flow_ER1"><di:waypoint x="216" y="198"/><di:waypoint x="280" y="198"/></bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_ER2" bpmnElement="Flow_ER2"><di:waypoint x="380" y="198"/><di:waypoint x="450" y="198"/></bpmndi:BPMNEdge>
      <bpmndi:BPMNEdge id="Edge_ER3" bpmnElement="Flow_ER3"><di:waypoint x="550" y="198"/><di:waypoint x="620" y="198"/></bpmndi:BPMNEdge>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>', 0, NULL, 0, NULL, NULL, NULL, '2026-06-16 22:19:12.609055+08', '2026-06-16 22:19:12.609055+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (275, 'Process_LeaveRequest', '1', '请假申请', 'LeaveRequest', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?>
<bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn">
  <bpmn:process id="Process_LeaveRequest" sf:code="LeaveRequest" name="请假申请" isExecutable="true" sf:version="1">
    <bpmn:startEvent id="StartNode_LR" sf:code="Start" name="开始">
      <bpmn:outgoing>Flow_LR1</bpmn:outgoing>
    </bpmn:startEvent>
    <bpmn:task id="TaskNode_LR1" sf:code="task001" name="提交申请">
      <bpmn:incoming>Flow_LR1</bpmn:incoming>
      <bpmn:outgoing>Flow_LR2</bpmn:outgoing>
    </bpmn:task>
    <bpmn:exclusiveGateway id="GatewayNode_LR1" sf:code="xorsplit001" name="天数判断">
      <bpmn:incoming>Flow_LR2</bpmn:incoming>
      <bpmn:outgoing>Flow_LR3</bpmn:outgoing>
      <bpmn:outgoing>Flow_LR4</bpmn:outgoing>
    </bpmn:exclusiveGateway>
    <bpmn:task id="TaskNode_LR2" sf:code="task010" name="主管审批">
      <bpmn:incoming>Flow_LR3</bpmn:incoming>
      <bpmn:outgoing>Flow_LR5</bpmn:outgoing>
    </bpmn:task>
    <bpmn:task id="TaskNode_LR3" sf:code="task020" name="CEO审批">
      <bpmn:incoming>Flow_LR4</bpmn:incoming>
      <bpmn:outgoing>Flow_LR6</bpmn:outgoing>
    </bpmn:task>
    <bpmn:exclusiveGateway id="GatewayNode_LR2" sf:code="xorjoin001" name="审批汇总">
      <bpmn:incoming>Flow_LR5</bpmn:incoming>
      <bpmn:incoming>Flow_LR6</bpmn:incoming>
      <bpmn:outgoing>Flow_LR7</bpmn:outgoing>
    </bpmn:exclusiveGateway>
    <bpmn:task id="TaskNode_LR4" sf:code="task100" name="HR审批">
      <bpmn:incoming>Flow_LR7</bpmn:incoming>
      <bpmn:outgoing>Flow_LR8</bpmn:outgoing>
    </bpmn:task>
    <bpmn:endEvent id="EndNode_LR" sf:code="End" name="结束">
      <bpmn:incoming>Flow_LR8</bpmn:incoming>
    </bpmn:endEvent>
    <bpmn:sequenceFlow id="Flow_LR1" sourceRef="StartNode_LR" targetRef="TaskNode_LR1" />
    <bpmn:sequenceFlow id="Flow_LR2" sourceRef="TaskNode_LR1" targetRef="GatewayNode_LR1" />
    <bpmn:sequenceFlow id="Flow_LR3" name="days&lt;3" sourceRef="GatewayNode_LR1" targetRef="TaskNode_LR2">
      <bpmn:conditionExpression>days&lt;3</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:sequenceFlow id="Flow_LR4" name="days&gt;=3" sourceRef="GatewayNode_LR1" targetRef="TaskNode_LR3">
      <bpmn:conditionExpression>days&gt;=3</bpmn:conditionExpression>
    </bpmn:sequenceFlow>
    <bpmn:sequenceFlow id="Flow_LR5" sourceRef="TaskNode_LR2" targetRef="GatewayNode_LR2" />
    <bpmn:sequenceFlow id="Flow_LR6" sourceRef="TaskNode_LR3" targetRef="GatewayNode_LR2" />
    <bpmn:sequenceFlow id="Flow_LR7" sourceRef="GatewayNode_LR2" targetRef="TaskNode_LR4" />
    <bpmn:sequenceFlow id="Flow_LR8" sourceRef="TaskNode_LR4" targetRef="EndNode_LR" />
  </bpmn:process>
  <bpmndi:BPMNDiagram id="BPMNDiagram_LR">
    <bpmndi:BPMNPlane id="BPMNPlane_LR" bpmnElement="Process_LeaveRequest">
      <bpmndi:BPMNShape id="Shape_StartNode_LR" bpmnElement="StartNode_LR"><dc:Bounds x="240" y="180" width="36" height="36"/></bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_TaskNode_LR1" bpmnElement="TaskNode_LR1"><dc:Bounds x="356" y="158" width="100" height="80"/></bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_GatewayNode_LR1" bpmnElement="GatewayNode_LR1" isMarkerVisible="true"><dc:Bounds x="536" y="180" width="36" height="36"/></bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_TaskNode_LR2" bpmnElement="TaskNode_LR2"><dc:Bounds x="652" y="230" width="100" height="80"/></bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_TaskNode_LR3" bpmnElement="TaskNode_LR3"><dc:Bounds x="652" y="70" width="100" height="80"/></bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_GatewayNode_LR2" bpmnElement="GatewayNode_LR2" isMarkerVisible="true"><dc:Bounds x="832" y="180" width="36" height="36"/></bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_TaskNode_LR4" bpmnElement="TaskNode_LR4"><dc:Bounds x="948" y="158" width="100" height="80"/></bpmndi:BPMNShape>
      <bpmndi:BPMNShape id="Shape_EndNode_LR" bpmnElement="EndNode_LR"><dc:Bounds x="1128" y="180" width="36" height="36"/></bpmndi:BPMNShape>
    </bpmndi:BPMNPlane>
  </bpmndi:BPMNDiagram>
</bpmn:definitions>', 0, NULL, 0, NULL, NULL, NULL, '2026-06-17 11:59:22.764084+08', '2026-06-17 11:59:22.764084+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (276, 'Process_TravelSubsidy_n1l8', '1', '出差补贴申请流程', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="Process_TravelSubsidy_n1l8" name="出差补贴申请流程" isExecutable="false"><bpmn:startEvent id="AStartEvent" name="开始" /><bpmn:userTask id="ATask_Apply" name="填写出差补贴申请单" /><bpmn:exclusiveGateway id="AGateway_Manager" name="部门主管审批" /><bpmn:userTask id="ATask_Finance" name="财务复核" /><bpmn:serviceTask id="ATask_Payment" name="系统自动打款" /><bpmn:endEvent id="AEndEvent_Approve" name="补贴发放完成" /><bpmn:endEvent id="AEndEvent_Reject" name="申请被驳回" /><bpmn:sequenceFlow id="Flow_AStartEvent_ATask_Apply" sourceRef="AStartEvent" targetRef="ATask_Apply" /><bpmn:sequenceFlow id="Flow_ATask_Apply_AGateway_Manager" sourceRef="ATask_Apply" targetRef="AGateway_Manager" /><bpmn:sequenceFlow id="Flow_AGateway_Manager_ATask_Finance" sourceRef="AGateway_Manager" targetRef="ATask_Finance" /><bpmn:sequenceFlow id="Flow_AGateway_Manager_AEndEvent_Reject" sourceRef="AGateway_Manager" targetRef="AEndEvent_Reject" /><bpmn:sequenceFlow id="Flow_ATask_Finance_ATask_Payment" sourceRef="ATask_Finance" targetRef="ATask_Payment" /><bpmn:sequenceFlow id="Flow_ATask_Payment_AEndEvent_Approve" sourceRef="ATask_Payment" targetRef="AEndEvent_Approve" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_TravelSubsidy_n1l8"><bpmndi:BPMNShape id="Shape_AStartEvent" bpmnElement="AStartEvent"><dc:Bounds x="150" y="162" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_Apply" bpmnElement="ATask_Apply"><dc:Bounds x="350" y="140" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AGateway_Manager" bpmnElement="AGateway_Manager"><dc:Bounds x="550" y="162" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_Finance" bpmnElement="ATask_Finance"><dc:Bounds x="750" y="301" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_Payment" bpmnElement="ATask_Payment"><dc:Bounds x="950" y="301" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent_Approve" bpmnElement="AEndEvent_Approve"><dc:Bounds x="1150" y="323" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent_Reject" bpmnElement="AEndEvent_Reject"><dc:Bounds x="750" y="140" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AStartEvent_ATask_Apply" bpmnElement="Flow_AStartEvent_ATask_Apply"><di:waypoint x="186" y="180" /><di:waypoint x="236" y="180" /><di:waypoint x="236" y="180" /><di:waypoint x="350" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_Apply_AGateway_Manager" bpmnElement="Flow_ATask_Apply_AGateway_Manager"><di:waypoint x="450" y="180" /><di:waypoint x="500" y="180" /><di:waypoint x="500" y="180" /><di:waypoint x="550" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_Manager_ATask_Finance" bpmnElement="Flow_AGateway_Manager_ATask_Finance"><di:waypoint x="586" y="180" /><di:waypoint x="636" y="180" /><di:waypoint x="636" y="341" /><di:waypoint x="750" y="341" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_Manager_AEndEvent_Reject" bpmnElement="Flow_AGateway_Manager_AEndEvent_Reject"><di:waypoint x="586" y="180" /><di:waypoint x="636" y="180" /><di:waypoint x="636" y="158" /><di:waypoint x="750" y="158" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_Finance_ATask_Payment" bpmnElement="Flow_ATask_Finance_ATask_Payment"><di:waypoint x="850" y="341" /><di:waypoint x="900" y="341" /><di:waypoint x="950" y="341" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_Payment_AEndEvent_Approve" bpmnElement="Flow_ATask_Payment_AEndEvent_Approve"><di:waypoint x="1050" y="341" /><di:waypoint x="1100" y="341" /><di:waypoint x="1100" y="341" /><di:waypoint x="1150" y="341" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, 0, NULL, NULL, NULL, '2026-06-19 14:32:04.846935+08', '2026-06-19 14:32:04.846935+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (277, '481d77c1-5274-4158-aece-cf5487df8a59', '1', 'test001', 'testcode', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="UTF-8"?><bpmn:definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:sf="http://www.slickflow.com/schema/sf" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL" id="bpmn-diagram" targetNamespace="http://bpmn.io/schema/bpmn" schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL BPMN20.xsd"><bpmn:process id="481d77c1-5274-4158-aece-cf5487df8a59" name="test001" isExecutable="true" sf:code="testcode" sf:version="1"><bpmn:startEvent id="StartEvent_1" name="Start" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="481d77c1-5274-4158-aece-cf5487df8a59"><bpmndi:BPMNShape id="_BPMNShape_StartEvent_2" bpmnElement="StartEvent_1"><dc:Bounds height="36.0" width="36.0" x="412.0" y="240.0" /></bpmndi:BPMNShape></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, 0, NULL, NULL, NULL, '2026-06-19 15:02:56.300491+08', NULL, NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (278, 'Process_LeaveApproval_rr2x', '1', '员工请假审批流程', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="Process_LeaveApproval_rr2x" name="员工请假审批流程" isExecutable="false"><bpmn:startEvent id="AStartEvent" name="开始" /><bpmn:userTask id="ATask_Submit" name="提交请假申请" /><bpmn:userTask id="ATask_Approve" name="主管审批" /><bpmn:exclusiveGateway id="AGateway_Check" name="是否批准?" /><bpmn:serviceTask id="ATask_Record" name="系统记录假期" /><bpmn:endEvent id="AEndEvent_Pass" name="请假通过" /><bpmn:endEvent id="AEndEvent_Reject" name="请假驳回" /><bpmn:sequenceFlow id="Flow_AStartEvent_ATask_Submit" sourceRef="AStartEvent" targetRef="ATask_Submit" /><bpmn:sequenceFlow id="Flow_ATask_Submit_ATask_Approve" sourceRef="ATask_Submit" targetRef="ATask_Approve" /><bpmn:sequenceFlow id="Flow_ATask_Approve_AGateway_Check" sourceRef="ATask_Approve" targetRef="AGateway_Check" /><bpmn:sequenceFlow id="Flow_AGateway_Check_ATask_Record" sourceRef="AGateway_Check" targetRef="ATask_Record" /><bpmn:sequenceFlow id="Flow_AGateway_Check_AEndEvent_Reject" sourceRef="AGateway_Check" targetRef="AEndEvent_Reject" /><bpmn:sequenceFlow id="Flow_ATask_Record_AEndEvent_Pass" sourceRef="ATask_Record" targetRef="AEndEvent_Pass" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_LeaveApproval_rr2x"><bpmndi:BPMNShape id="Shape_AStartEvent" bpmnElement="AStartEvent"><dc:Bounds x="150" y="162" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_Submit" bpmnElement="ATask_Submit"><dc:Bounds x="350" y="140" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_Approve" bpmnElement="ATask_Approve"><dc:Bounds x="550" y="140" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AGateway_Check" bpmnElement="AGateway_Check"><dc:Bounds x="750" y="162" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_Record" bpmnElement="ATask_Record"><dc:Bounds x="950" y="301" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent_Pass" bpmnElement="AEndEvent_Pass"><dc:Bounds x="1150" y="323" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent_Reject" bpmnElement="AEndEvent_Reject"><dc:Bounds x="950" y="140" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AStartEvent_ATask_Submit" bpmnElement="Flow_AStartEvent_ATask_Submit"><di:waypoint x="186" y="180" /><di:waypoint x="236" y="180" /><di:waypoint x="236" y="180" /><di:waypoint x="350" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_Submit_ATask_Approve" bpmnElement="Flow_ATask_Submit_ATask_Approve"><di:waypoint x="450" y="180" /><di:waypoint x="500" y="180" /><di:waypoint x="550" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_Approve_AGateway_Check" bpmnElement="Flow_ATask_Approve_AGateway_Check"><di:waypoint x="650" y="180" /><di:waypoint x="700" y="180" /><di:waypoint x="700" y="180" /><di:waypoint x="750" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_Check_ATask_Record" bpmnElement="Flow_AGateway_Check_ATask_Record"><di:waypoint x="786" y="180" /><di:waypoint x="836" y="180" /><di:waypoint x="836" y="341" /><di:waypoint x="950" y="341" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_Check_AEndEvent_Reject" bpmnElement="Flow_AGateway_Check_AEndEvent_Reject"><di:waypoint x="786" y="180" /><di:waypoint x="836" y="180" /><di:waypoint x="836" y="158" /><di:waypoint x="950" y="158" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_Record_AEndEvent_Pass" bpmnElement="Flow_ATask_Record_AEndEvent_Pass"><di:waypoint x="1050" y="341" /><di:waypoint x="1100" y="341" /><di:waypoint x="1100" y="341" /><di:waypoint x="1150" y="341" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, 0, NULL, NULL, NULL, '2026-06-20 11:26:27.35735+08', '2026-06-20 11:26:27.35735+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (279, 'Process_Expense_Reimbursement_wvhk', '1', '费用报销审批流程', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="Process_Expense_Reimbursement_wvhk" name="费用报销审批流程" isExecutable="false"><bpmn:startEvent id="AStartEvent" name="开始" /><bpmn:userTask id="ATask_Submit" name="提交报销申请" /><bpmn:userTask id="ATask_Manager_Approve" name="部门经理审批" /><bpmn:exclusiveGateway id="AGateway_Manager" name="经理审批判断" /><bpmn:userTask id="ATask_Finance_Audit" name="财务审核" /><bpmn:exclusiveGateway id="AGateway_Finance" name="财务审核判断" /><bpmn:serviceTask id="ATask_Payment" name="出纳打款" /><bpmn:endEvent id="AEndEvent_Success" name="报销完成" /><bpmn:endEvent id="AEndEvent_Rejected" name="审批驳回" /><bpmn:sequenceFlow id="Flow_AStartEvent_ATask_Submit" sourceRef="AStartEvent" targetRef="ATask_Submit" /><bpmn:sequenceFlow id="Flow_ATask_Submit_ATask_Manager_Approve" sourceRef="ATask_Submit" targetRef="ATask_Manager_Approve" /><bpmn:sequenceFlow id="Flow_ATask_Manager_Approve_AGateway_Manager" sourceRef="ATask_Manager_Approve" targetRef="AGateway_Manager" /><bpmn:sequenceFlow id="Flow_AGateway_Manager_ATask_Finance_Audit" sourceRef="AGateway_Manager" targetRef="ATask_Finance_Audit" /><bpmn:sequenceFlow id="Flow_AGateway_Manager_AEndEvent_Rejected" sourceRef="AGateway_Manager" targetRef="AEndEvent_Rejected" /><bpmn:sequenceFlow id="Flow_ATask_Finance_Audit_AGateway_Finance" sourceRef="ATask_Finance_Audit" targetRef="AGateway_Finance" /><bpmn:sequenceFlow id="Flow_AGateway_Finance_ATask_Payment" sourceRef="AGateway_Finance" targetRef="ATask_Payment" /><bpmn:sequenceFlow id="Flow_AGateway_Finance_AEndEvent_Rejected" sourceRef="AGateway_Finance" targetRef="AEndEvent_Rejected" /><bpmn:sequenceFlow id="Flow_ATask_Payment_AEndEvent_Success" sourceRef="ATask_Payment" targetRef="AEndEvent_Success" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="Process_Expense_Reimbursement_wvhk"><bpmndi:BPMNShape id="Shape_AStartEvent" bpmnElement="AStartEvent"><dc:Bounds x="150" y="162" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_Submit" bpmnElement="ATask_Submit"><dc:Bounds x="350" y="140" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_Manager_Approve" bpmnElement="ATask_Manager_Approve"><dc:Bounds x="550" y="140" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AGateway_Manager" bpmnElement="AGateway_Manager"><dc:Bounds x="750" y="162" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_Finance_Audit" bpmnElement="ATask_Finance_Audit"><dc:Bounds x="950" y="261" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AGateway_Finance" bpmnElement="AGateway_Finance"><dc:Bounds x="1150" y="283" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ATask_Payment" bpmnElement="ATask_Payment"><dc:Bounds x="1350" y="301" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent_Success" bpmnElement="AEndEvent_Success"><dc:Bounds x="1550" y="323" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent_Rejected" bpmnElement="AEndEvent_Rejected"><dc:Bounds x="1350" y="140" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AStartEvent_ATask_Submit" bpmnElement="Flow_AStartEvent_ATask_Submit"><di:waypoint x="186" y="180" /><di:waypoint x="236" y="180" /><di:waypoint x="236" y="180" /><di:waypoint x="350" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_Submit_ATask_Manager_Approve" bpmnElement="Flow_ATask_Submit_ATask_Manager_Approve"><di:waypoint x="450" y="180" /><di:waypoint x="500" y="180" /><di:waypoint x="550" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_Manager_Approve_AGateway_Manager" bpmnElement="Flow_ATask_Manager_Approve_AGateway_Manager"><di:waypoint x="650" y="180" /><di:waypoint x="700" y="180" /><di:waypoint x="700" y="180" /><di:waypoint x="750" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_Manager_ATask_Finance_Audit" bpmnElement="Flow_AGateway_Manager_ATask_Finance_Audit"><di:waypoint x="786" y="180" /><di:waypoint x="836" y="180" /><di:waypoint x="836" y="301" /><di:waypoint x="950" y="301" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_Manager_AEndEvent_Rejected" bpmnElement="Flow_AGateway_Manager_AEndEvent_Rejected"><di:waypoint x="786" y="180" /><di:waypoint x="836" y="180" /><di:waypoint x="836" y="158" /><di:waypoint x="1350" y="158" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_Finance_Audit_AGateway_Finance" bpmnElement="Flow_ATask_Finance_Audit_AGateway_Finance"><di:waypoint x="1050" y="301" /><di:waypoint x="1100" y="301" /><di:waypoint x="1100" y="301" /><di:waypoint x="1150" y="301" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_Finance_ATask_Payment" bpmnElement="Flow_AGateway_Finance_ATask_Payment"><di:waypoint x="1186" y="301" /><di:waypoint x="1236" y="301" /><di:waypoint x="1236" y="341" /><di:waypoint x="1350" y="341" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_Finance_AEndEvent_Rejected" bpmnElement="Flow_AGateway_Finance_AEndEvent_Rejected"><di:waypoint x="1186" y="301" /><di:waypoint x="1236" y="301" /><di:waypoint x="1236" y="158" /><di:waypoint x="1350" y="158" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ATask_Payment_AEndEvent_Success" bpmnElement="Flow_ATask_Payment_AEndEvent_Success"><di:waypoint x="1450" y="341" /><di:waypoint x="1500" y="341" /><di:waypoint x="1500" y="341" /><di:waypoint x="1550" y="341" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, 0, NULL, NULL, NULL, '2026-06-20 11:52:08.158335+08', '2026-06-20 11:52:08.158336+08', NULL);
INSERT INTO public.wf_process (id, process_id, version, process_name, process_code, status, app_type, package_type, package_id, participant_guid, page_url, xml_file_name, xml_file_path, xml_content, start_type, start_expression, end_type, end_expression, icon, description, created_datetime, updated_datetime, row_version_id) VALUES (280, 'PO_Approval_Process_e5cs', '1', '采购订单审批流程', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '<?xml version="1.0" encoding="utf-8"?><bpmn:definitions xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:dc="http://www.omg.org/spec/DD/20100524/DC" xmlns:di="http://www.omg.org/spec/DD/20100524/DI" xmlns:bpmn="http://www.omg.org/spec/BPMN/20100524/MODEL"><bpmn:process id="PO_Approval_Process_e5cs" name="采购订单审批流程" isExecutable="false"><bpmn:startEvent id="AStartEvent" name="开始" /><bpmn:userTask id="ASubmit_PO" name="提交采购订单" /><bpmn:userTask id="AManager_Approval" name="部门经理审批" /><bpmn:exclusiveGateway id="AGateway_Manager" name="经理审批是否通过?" /><bpmn:userTask id="AFinance_Approval" name="财务审批" /><bpmn:exclusiveGateway id="AGateway_Finance" name="财务审批是否通过?" /><bpmn:serviceTask id="AGenerate_PO" name="生成正式采购订单" /><bpmn:endEvent id="AEndEvent_Approved" name="流程结束(通过)" /><bpmn:endEvent id="AEndEvent_Rejected" name="流程结束(拒绝)" /><bpmn:sequenceFlow id="Flow_AStartEvent_ASubmit_PO" sourceRef="AStartEvent" targetRef="ASubmit_PO" /><bpmn:sequenceFlow id="Flow_ASubmit_PO_AManager_Approval" sourceRef="ASubmit_PO" targetRef="AManager_Approval" /><bpmn:sequenceFlow id="Flow_AManager_Approval_AGateway_Manager" sourceRef="AManager_Approval" targetRef="AGateway_Manager" /><bpmn:sequenceFlow id="Flow_AGateway_Manager_AFinance_Approval" sourceRef="AGateway_Manager" targetRef="AFinance_Approval" /><bpmn:sequenceFlow id="Flow_AGateway_Manager_AEndEvent_Rejected" sourceRef="AGateway_Manager" targetRef="AEndEvent_Rejected" /><bpmn:sequenceFlow id="Flow_AFinance_Approval_AGateway_Finance" sourceRef="AFinance_Approval" targetRef="AGateway_Finance" /><bpmn:sequenceFlow id="Flow_AGateway_Finance_AGenerate_PO" sourceRef="AGateway_Finance" targetRef="AGenerate_PO" /><bpmn:sequenceFlow id="Flow_AGateway_Finance_AEndEvent_Rejected" sourceRef="AGateway_Finance" targetRef="AEndEvent_Rejected" /><bpmn:sequenceFlow id="Flow_AGenerate_PO_AEndEvent_Approved" sourceRef="AGenerate_PO" targetRef="AEndEvent_Approved" /></bpmn:process><bpmndi:BPMNDiagram id="BPMNDiagram_1"><bpmndi:BPMNPlane id="BPMNPlane_1" bpmnElement="PO_Approval_Process_e5cs"><bpmndi:BPMNShape id="Shape_AStartEvent" bpmnElement="AStartEvent"><dc:Bounds x="150" y="162" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_ASubmit_PO" bpmnElement="ASubmit_PO"><dc:Bounds x="350" y="140" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AManager_Approval" bpmnElement="AManager_Approval"><dc:Bounds x="550" y="140" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AGateway_Manager" bpmnElement="AGateway_Manager"><dc:Bounds x="750" y="162" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AFinance_Approval" bpmnElement="AFinance_Approval"><dc:Bounds x="950" y="261" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AGateway_Finance" bpmnElement="AGateway_Finance"><dc:Bounds x="1150" y="283" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AGenerate_PO" bpmnElement="AGenerate_PO"><dc:Bounds x="1350" y="301" width="100" height="80" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent_Approved" bpmnElement="AEndEvent_Approved"><dc:Bounds x="1550" y="323" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNShape id="Shape_AEndEvent_Rejected" bpmnElement="AEndEvent_Rejected"><dc:Bounds x="1350" y="140" width="36" height="36" /></bpmndi:BPMNShape><bpmndi:BPMNEdge id="Edge_AStartEvent_ASubmit_PO" bpmnElement="Flow_AStartEvent_ASubmit_PO"><di:waypoint x="186" y="180" /><di:waypoint x="236" y="180" /><di:waypoint x="236" y="180" /><di:waypoint x="350" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_ASubmit_PO_AManager_Approval" bpmnElement="Flow_ASubmit_PO_AManager_Approval"><di:waypoint x="450" y="180" /><di:waypoint x="500" y="180" /><di:waypoint x="550" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AManager_Approval_AGateway_Manager" bpmnElement="Flow_AManager_Approval_AGateway_Manager"><di:waypoint x="650" y="180" /><di:waypoint x="700" y="180" /><di:waypoint x="700" y="180" /><di:waypoint x="750" y="180" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_Manager_AFinance_Approval" bpmnElement="Flow_AGateway_Manager_AFinance_Approval"><di:waypoint x="786" y="180" /><di:waypoint x="836" y="180" /><di:waypoint x="836" y="301" /><di:waypoint x="950" y="301" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_Manager_AEndEvent_Rejected" bpmnElement="Flow_AGateway_Manager_AEndEvent_Rejected"><di:waypoint x="786" y="180" /><di:waypoint x="836" y="180" /><di:waypoint x="836" y="158" /><di:waypoint x="1350" y="158" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AFinance_Approval_AGateway_Finance" bpmnElement="Flow_AFinance_Approval_AGateway_Finance"><di:waypoint x="1050" y="301" /><di:waypoint x="1100" y="301" /><di:waypoint x="1100" y="301" /><di:waypoint x="1150" y="301" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_Finance_AGenerate_PO" bpmnElement="Flow_AGateway_Finance_AGenerate_PO"><di:waypoint x="1186" y="301" /><di:waypoint x="1236" y="301" /><di:waypoint x="1236" y="341" /><di:waypoint x="1350" y="341" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGateway_Finance_AEndEvent_Rejected" bpmnElement="Flow_AGateway_Finance_AEndEvent_Rejected"><di:waypoint x="1186" y="301" /><di:waypoint x="1236" y="301" /><di:waypoint x="1236" y="158" /><di:waypoint x="1350" y="158" /></bpmndi:BPMNEdge><bpmndi:BPMNEdge id="Edge_AGenerate_PO_AEndEvent_Approved" bpmnElement="Flow_AGenerate_PO_AEndEvent_Approved"><di:waypoint x="1450" y="341" /><di:waypoint x="1500" y="341" /><di:waypoint x="1500" y="341" /><di:waypoint x="1550" y="341" /></bpmndi:BPMNEdge></bpmndi:BPMNPlane></bpmndi:BPMNDiagram></bpmn:definitions>', 0, NULL, 0, NULL, NULL, NULL, '2026-06-20 19:29:19.051533+08', '2026-06-20 19:29:19.051533+08', NULL);


--
-- Data for Name: wf_variable; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.wf_variable (id, process_id, version, activity_id, name, type, direction, default_value, is_required, is_referenced, source_ref, source_variable_name, sort_order, description, created_datetime, updated_datetime) VALUES (1, 'process_2925_2556', '1', 'TaskNode_5628', '', '', 'Input', '', 0, 0, '', '', 0, NULL, '2026-04-23 17:03:27.666061+08', NULL);
INSERT INTO public.wf_variable (id, process_id, version, activity_id, name, type, direction, default_value, is_required, is_referenced, source_ref, source_variable_name, sort_order, description, created_datetime, updated_datetime) VALUES (2, 'process_2925_2556', '1', 'TaskNode_5628', '', '', 'Input', '', 0, 0, '', '', 1, NULL, '2026-04-23 17:03:27.683465+08', NULL);
INSERT INTO public.wf_variable (id, process_id, version, activity_id, name, type, direction, default_value, is_required, is_referenced, source_ref, source_variable_name, sort_order, description, created_datetime, updated_datetime) VALUES (3, 'process_2925_2556', '1', 'TaskNode_5628', '', '', 'Input', '', 0, 0, '', '', 2, NULL, '2026-04-23 17:03:27.685576+08', NULL);
INSERT INTO public.wf_variable (id, process_id, version, activity_id, name, type, direction, default_value, is_required, is_referenced, source_ref, source_variable_name, sort_order, description, created_datetime, updated_datetime) VALUES (4, 'process_6399_6135', '1', 'TaskNode_5628', 'out_name', 'String', 'Output', '', 0, 0, '', '', 0, NULL, '2026-05-07 09:38:51.581158+08', NULL);


--
-- Name: ai_activity_config_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.ai_activity_config_id_seq', 25, true);


--
-- Name: ai_model_provider_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.ai_model_provider_id_seq', 1, false);


--
-- Name: wf_variable_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.wf_variable_id_seq', 4, true);


--
-- PostgreSQL database dump complete
--

\unrestrict dXJfhSXhRKM8MMVM092k194hFuG4udZ2IiZUjXVwOutcVRL5yCihdybLSkiR68m


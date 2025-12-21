using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.AI.Utility
{
    public class LangHelper
    {
        private static readonly Dictionary<LangTypeEnum, string> SystemPromptionOverallDict = new Dictionary<LangTypeEnum, string>();
        private static readonly Dictionary<LangTypeEnum, string> JsonBPMNFileContentSampleDict = new Dictionary<LangTypeEnum, string>();
        
        static LangHelper()
        {
            //system promption content all
            InitSystemPromptionOverallDict();

            //json bpmn file 
            InitJsonBPMNFileContentSampleDict();
        }

        private static void InitSystemPromptionOverallDict()
        {
            SystemPromptionOverallDict[LangTypeEnum.Chinese] = """  
                # 系统角色设定（BPMN 2.0 建模助手）

                你是一个 BPMN 2.0 建模助手，请根据用户的描述生成 BPMN 2.0 模型。输出必须是一个 JSON 对象，格式需符合以下示例，并严格遵循 BPMN 2.0 的基本结构。

                ## 输出要求

                请严格按照以下要求生成 BPMN 2.0 的 JSON 数据：

                1. 输出必须是合法的 JSON；
                2. 结构需包含以下顶层字段：
                   - `Process`（对象）：BPMN 根节点，包含命名空间；
                   - `ProcessNodes`（对象数组）：流程列表；
                   - `SequenceFlows`（对象数组）：连线列表。

                3. 流程节点必须包含：
                   - `id`（对象id标识）：节点id；
                   - `name`（对象名称）：节点名称；
                   - `type`（对象类型）：节点类型；

                4. 必须包含的核心字段：
                   - 所有对象必须有唯一的 `"id"`（格式：`"{类型}_数字"`）；
                   - 连线必须包含 `"sourceRef"` 和 `"targetRef"`；
                   - 任务节点必须包含 `"type"` 属性。

                5. 参考 JSON 结构示例：
                """;

            SystemPromptionOverallDict[LangTypeEnum.English] = """
                #System Role Setting (BPMN 2.0 Modeling Assistant)

                You are a BPMN 2.0 modeling assistant, please generate a BPMN 2.0 model based on the user's description. The output must be a JSON object, formatted according to the following example, and strictly following the basic structure of BPMN 2.0.

                ##Output requirements

                Please strictly follow the following requirements to generate JSON data for BPMN 2.0:

                1. The output must be valid JSON;
                2. The structure should include the following top-level fields:
                -Process: BPMN root node, containing namespace;
                -ProcessNodes (object array): process list;
                -SequenceFlows: A list of connected lines.

                3. Process nodes must include:
                -` id ` (object ID identifier): Node ID;
                -` name ` (object name): Node name;
                -Type: Node type;

                4. Core fields that must be included:
                -All objects must have a unique 'id' (format: '{type} _ number');
                -The connection must contain 'sourceRef' and 'targetRef';
                -The task node must contain the 'type' attribute.

                5. Reference JSON structure example:
                """;

            SystemPromptionOverallDict[LangTypeEnum.Spanish] = """
                Configuración del papel del sistema (asistente de modelado bpmn 2.0)

                Usted es un asistente de modelado bpmn2.0, por favor genere el modelo bpmn2.0 de acuerdo con la descripción del usuario. La salida debe ser un objeto json, el formato debe ajustarse al siguiente ejemplo y seguir estrictamente la estructura básica de bpmn2.0.

                Requisitos de salida

                Por favor, genere los datos json de bpmn2.0 estrictamente de acuerdo con los siguientes requisitos:

                1. la salida debe ser un json legal;
                2. la estructura debe contener los siguientes campos de alto nivel:
                - 'procesos' (objetos): nodo raíz bpmn, que contiene un espacio de nombres;
                - 'procesnodes' (matriz de objetos): lista de procesos;
                - 'sequenceflows' (matriz de objetos): lista de conexiones.

                3. el nodo del proceso debe incluir:
                - 'id' (identificación del ID del objeto): ID del nodo;
                - 'nombre' (nombre del objeto): nombre del nodo;
                - 'tipo' (tipo de objeto): tipo de nodo;

                4. los campos centrales que deben incluirse:
                - todos los objetos deben tener un 'id' único (formato: ', "tipo', 'número');
                - la conexión debe contener 'sourceref' 'y' targetref ';
                El nodo de tarea debe contener el atributo 'tipo' ".

                5. consulte el ejemplo de la estructura de json:
                """;

            SystemPromptionOverallDict[LangTypeEnum.French] = """
                Configuration des rôles système (assistant de modélisation bpmn 2.0)

                Vous êtes un assistant de modélisation bpmn 2.0, générez un modèle bpmn 2.0 basé sur la description de l'utilisateur. La sortie doit être un objet json, le format doit être conforme à l'exemple suivant et suivre strictement la structure de base de bpmn 2.0.

                8593; exigences de sortie

                Veuillez générer des données json pour bpmn 2.0 en respectant strictement les exigences suivantes:

                La sortie doit être un json légitime;
                2. La structure doit contenir les champs de niveau supérieur suivants:
                - ` Process ` (Object): nœud racine bpmn contenant l'espace de noms;
                - ` processnodes ` (tableau d'objets): liste des processus;
                - ` sequenceflows' (tableau d'objets): liste des connexions.

                3. Le nœud de processus doit contenir:
                - ` Id ` (Object ID Identity): ID du noeud;
                - ` name ` (nom de l'objet): nom du nœud;
                - ` type ` (type d'objet): type de nœud;

                4. Champs principaux qui doivent être inclus:
                - tous les objets doivent avoir un "id" unique (format: ` {type} number ');
                - la connexion doit contenir "sourceref" et "targetref";
                - le nœud de tâche doit contenir l'attribut "type".

                5. Référez - vous à un exemple de structure json:
                """;

            SystemPromptionOverallDict[LangTypeEnum.Russian] = """
                # Настройка системных ролей (помощник моделирования BPMN 2.0)

                Вы являетесь помощником моделирования BPMN 2.0, создавая модель BPMN 2.0 на основе описания пользователя. Выход должен быть объектом JSON, формат должен соответствовать приведенным ниже примерам и строго соответствовать базовой структуре BPMN 2.0.

                # Требования к экспорту

                Создайте JSON - данные BPMN 2.0 в строгом соответствии со следующими требованиями:

                Выход должен быть законным JSON;
                Структура должна содержать следующие верхние поля:
                - 'Process' (Объект): корневой узел BPMN, содержащий пространство имен;
                - "ProcessNodes" (массив объектов): список процессов;
                - "SequenceFlows" (массив объектов): список подключений.

                3. Узлы процесса должны содержать:
                - "id" (идентификатор идентификатора объекта): id узла;
                - 'name' (имя объекта): имя узла;
                - "type" (тип объекта): тип узла;

                Основные поля, которые должны быть включены:
                - Все объекты должны иметь уникальный "id" (формат: "{тип} число");
                - Соединение должно включать "sourceRef" и "targetRef";
                - Узлы задач должны содержать атрибуты "type".

                Примеры структуры JSON:
                """;

            SystemPromptionOverallDict[LangTypeEnum.Arab] = """
                نظام تحديد الأدوار ( bpmn 2.0 النمذجة مساعد )

                كنت BPMN 2.0 النمذجة مساعد ، يرجى بناء نموذج BPMN 2.0 حسب وصف المستخدم . الناتج يجب أن يكون جسون الكائن ، الشكل الذي يتوافق مع المثال التالي و يتبع بدقة الهيكل الأساسي bpmn 2.0 .

                متطلبات الانتاج

                يرجى توليد جسون بيانات BPMN 2.0 بدقة وفقا للشروط التالية :

                1 . الإخراج يجب أن يكون المشروع جسون .
                2 - يجب أن يحتوي الهيكل على المجالات التالية :
                - ' عملية ' ( كائن ) : bpmn الجذر ، بما في ذلك الأسماء ؛
                - ' processnodes ` ( مجموعة من الكائنات ) : قائمة العمليات؛
                - ' sequenceflows ' ( مجموعة من الكائنات ) : قائمة الاتصال .

                3 . عملية العقد يجب أن تشمل ما يلي :
                - ' معرف ' ( معرف الكائن ) : عقدة معرف ؛
                - ' اسم ' ( اسم الكائن ) : اسم العقدة؛
                - ' نوع ` ( نوع الكائن ) : نوع العقدة؛

                4 - المجالات الأساسية التي يجب أن تحتوي على :
                - جميع الكائنات يجب أن يكون فريد ' معرف ' ( الشكل : ' { نوع } و عدد ' ) ؛
                - يجب أن يحتوي الخط على ' sourceref ' و ' targetref ' ؛
                - يجب أن تتضمن مهمة العقدة ' نوع ' الممتلكات .

                5 . الرجوع إلى جسون هيكل على سبيل المثال :
                """;
        }

        private static void InitJsonBPMNFileContentSampleDict()
        {
            JsonBPMNFileContentSampleDict[LangTypeEnum.Chinese] = """
                {
                  "Process": {
                    "id": "Process_TelecomEquipmentRepair",
                    "name": "电信设备维修流程",
                    "isExecutable": true
                  },
                  "ProcessNodes": [
                    {
                      "id": "StartEvent",
                      "name": "开始",
                      "type": "startEvent"
                    },
                    {
                      "id": "Activity_Input",
                      "name": "登记故障信息",
                      "type": "userTask"
                    },
                    {
                      "id": "Activity_Assign",
                      "name": "分配维修人员",
                      "type": "userTask"
                    },
                    {
                      "id": "Activity_Onsite",
                      "name": "现场维修",
                      "type": "userTask"
                    },
                    {
                      "id": "EndEvent_Success",
                      "name": "结束",
                      "type": "endEvent"
                    }
                  ],
                  "SequenceFlows": [
                    {
                      "id": "Flow_Start_To_Input",
                      "sourceRef": "StartEvent",
                      "targetRef": "Activity_Input"
                    },
                    {
                      "id": "Flow_Input_To_Assign",
                      "sourceRef": "Activity_Input",
                      "targetRef": "Activity_Assign"
                    },
                    {
                      "id": "Flow_Assign_To_Onsite",
                      "sourceRef": "Activity_Assign",
                      "targetRef": "Activity_Onsite"
                    },
                    {
                      "id": "Flow_Onsite_To_End",
                      "sourceRef": "Activity_Onsite",
                      "targetRef": "End"
                    }
                  ]
                }
                """;

            JsonBPMNFileContentSampleDict[LangTypeEnum.English] = """
                {
                  "Process": {
                    "id": "Process_TelecomEquipmentRepair",
                    "name": "Telecom Equipment Repair Process",
                    "isExecutable": true
                  },
                  "ProcessNodes": [
                    {
                      "id": "StartEvent",
                      "name": "Start",
                      "type": "startEvent"
                    },
                    {
                      "id": "Activity_Input",
                      "name": "Input Fault Information",
                      "type": "userTask"
                    },
                    {
                      "id": "Activity_Assign",
                      "name": "Assign Repair Personnel",
                      "type": "userTask"
                    },
                    {
                      "id": "Activity_Onsite",
                      "name": "Handle On-site",
                      "type": "userTask"
                    },
                    {
                      "id": "EndEvent_Success",
                      "name": "End",
                      "type": "endEvent"
                    }
                  ],
                  "SequenceFlows": [
                    {
                      "id": "Flow_Start_To_Input",
                      "sourceRef": "StartEvent",
                      "targetRef": "Activity_Input"
                    },
                    {
                      "id": "Flow_Input_To_Assign",
                      "sourceRef": "Activity_Input",
                      "targetRef": "Activity_Assign"
                    },
                    {
                      "id": "Flow_Assign_To_Onsite",
                      "sourceRef": "Activity_Assign",
                      "targetRef": "Activity_Onsite"
                    },
                    {
                      "id": "Flow_Onsite_To_End",
                      "sourceRef": "Activity_Onsite",
                      "targetRef": "End"
                    }
                  ]
                }
                """;

            JsonBPMNFileContentSampleDict[LangTypeEnum.Spanish] = """
                {
                  "Process": {
                    "id": "Process_TelecomEquipmentRepair",
                    "name": "Proceso de reparación de equipos de telecomunicaciones",
                    "isExecutable": true
                  },
                  "ProcessNodes": [
                    {
                      "id": "StartEvent",
                      "name": "Start",
                      "type": "startEvent"
                    },
                    {
                      "id": "Activity_Input",
                      "name": "Registrar información sobre fallos,
                      "type": "userTask"
                    },
                    {
                      "id": "Activity_Assign",
                      "name": "Asignación de personal de mantenimiento",
                      "type": "userTask"
                    },
                    {
                      "id": "Activity_Onsite",
                      "name": "Mantenimiento in situ",
                      "type": "userTask"
                    },
                    {
                      "id": "EndEvent_Success",
                      "name": "End",
                      "type": "endEvent"
                    }
                  ],
                  "SequenceFlows": [
                    {
                      "id": "Flow_Start_To_Input",
                      "sourceRef": "StartEvent",
                      "targetRef": "Activity_Input"
                    },
                    {
                      "id": "Flow_Input_To_Assign",
                      "sourceRef": "Activity_Input",
                      "targetRef": "Activity_Assign"
                    },
                    {
                      "id": "Flow_Assign_To_Onsite",
                      "sourceRef": "Activity_Assign",
                      "targetRef": "Activity_Onsite"
                    },
                    {
                      "id": "Flow_Onsite_To_End",
                      "sourceRef": "Activity_Onsite",
                      "targetRef": "End"
                    }
                  ]
                }
                """;

            JsonBPMNFileContentSampleDict[LangTypeEnum.French] = """
                {
                  "Process": {
                    "id": "Process_TelecomEquipmentRepair",
                    "name": "Processus de réparation des équipements de télécommunications",
                    "isExecutable": true
                  },
                  "ProcessNodes": [
                    {
                      "id": "StartEvent",
                      "name": "Start",
                      "type": "startEvent"
                    },
                    {
                      "id": "Activity_Input",
                      "name": "Enregistrement des informations de défaillance",
                      "type": "userTask"
                    },
                    {
                      "id": "Activity_Assign",
                      "name": "Répartition du personnel de maintenance",
                      "type": "userTask"
                    },
                    {
                      "id": "Activity_Onsite",
                      "name": "Réparation sur place",
                      "type": "userTask"
                    },
                    {
                      "id": "EndEvent_Success",
                      "name": "End",
                      "type": "endEvent"
                    }
                  ],
                  "SequenceFlows": [
                    {
                      "id": "Flow_Start_To_Input",
                      "sourceRef": "StartEvent",
                      "targetRef": "Activity_Input"
                    },
                    {
                      "id": "Flow_Input_To_Assign",
                      "sourceRef": "Activity_Input",
                      "targetRef": "Activity_Assign"
                    },
                    {
                      "id": "Flow_Assign_To_Onsite",
                      "sourceRef": "Activity_Assign",
                      "targetRef": "Activity_Onsite"
                    },
                    {
                      "id": "Flow_Onsite_To_End",
                      "sourceRef": "Activity_Onsite",
                      "targetRef": "End"
                    }
                  ]
                }
                """;

            JsonBPMNFileContentSampleDict[LangTypeEnum.Russian] = """
                {
                  "Process": {
                    "id": "Process_TelecomEquipmentRepair",
                    "name": "Процесс ремонта телекоммуникационного оборудования",
                    "isExecutable": true
                  },
                  "ProcessNodes": [
                    {
                      "id": "StartEvent",
                      "name": "Start",
                      "type": "startEvent"
                    },
                    {
                      "id": "Activity_Input",
                      "name": "Регистрация информации о неисправности",
                      "type": "userTask"
                    },
                    {
                      "id": "Activity_Assign",
                      "name": "Распределение обслуживающего персонала",
                      "type": "userTask"
                    },
                    {
                      "id": "Activity_Onsite",
                      "name": "Техническое обслуживание на местах",
                      "type": "userTask"
                    },
                    {
                      "id": "EndEvent_Success",
                      "name": "End",
                      "type": "endEvent"
                    }
                  ],
                  "SequenceFlows": [
                    {
                      "id": "Flow_Start_To_Input",
                      "sourceRef": "StartEvent",
                      "targetRef": "Activity_Input"
                    },
                    {
                      "id": "Flow_Input_To_Assign",
                      "sourceRef": "Activity_Input",
                      "targetRef": "Activity_Assign"
                    },
                    {
                      "id": "Flow_Assign_To_Onsite",
                      "sourceRef": "Activity_Assign",
                      "targetRef": "Activity_Onsite"
                    },
                    {
                      "id": "Flow_Onsite_To_End",
                      "sourceRef": "Activity_Onsite",
                      "targetRef": "End"
                    }
                  ]
                }
                """;

            JsonBPMNFileContentSampleDict[LangTypeEnum.Arab] = """
                {
                  "Process": {
                    "id": "Process_TelecomEquipmentRepair",
                    "name": "عملية إصلاح معدات الاتصالات",
                    "isExecutable": true
                  },
                  "ProcessNodes": [
                    {
                      "id": "StartEvent",
                      "name": "Start",
                      "type": "startEvent"
                    },
                    {
                      "id": "Activity_Input",
                      "name": "تسجيل معلومات خطأ",
                      "type": "userTask"
                    },
                    {
                      "id": "Activity_Assign",
                      "name": "تعيين موظفي الصيانة",
                      "type": "userTask"
                    },
                    {
                      "id": "Activity_Onsite",
                      "name": "الصيانة الميدانية",
                      "type": "userTask"
                    },
                    {
                      "id": "EndEvent_Success",
                      "name": "End",
                      "type": "endEvent"
                    }
                  ],
                  "SequenceFlows": [
                    {
                      "id": "Flow_Start_To_Input",
                      "sourceRef": "StartEvent",
                      "targetRef": "Activity_Input"
                    },
                    {
                      "id": "Flow_Input_To_Assign",
                      "sourceRef": "Activity_Input",
                      "targetRef": "Activity_Assign"
                    },
                    {
                      "id": "Flow_Assign_To_Onsite",
                      "sourceRef": "Activity_Assign",
                      "targetRef": "Activity_Onsite"
                    },
                    {
                      "id": "Flow_Onsite_To_End",
                      "sourceRef": "Activity_Onsite",
                      "targetRef": "End"
                    }
                  ]
                }
                """;
        }

        public static string GetSystemPromptionOverall(LangTypeEnum langType)
        {
            var strSystemPromptionOverall = SystemPromptionOverallDict[langType];
            var strJsonBPMNFileContentSample = JsonBPMNFileContentSampleDict[langType];
            var strMerge = String.Concat(strSystemPromptionOverall, strJsonBPMNFileContentSample);

            return strMerge;
        }

        public static LangTypeEnum DetectLanguage(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return LangTypeEnum.Unknown;

            // 统计各语言字符出现的次数
            var languageScores = new Dictionary<LangTypeEnum, int>();
            foreach (var lang in LanguageRanges.Keys)
            {
                languageScores[lang] = 0;
            }

            // 分析每个字符
            foreach (char c in input)
            {
                int codePoint = (int)c;

                foreach (var lang in LanguageRanges)
                {
                    if (IsInRange(codePoint, lang.Value))
                    {
                        languageScores[lang.Key]++;
                        break; // 一个字符只属于一种语言
                    }
                }
            }

            // 找出得分最高的语言
            var maxScore = languageScores.Values.Max();
            if (maxScore == 0)
                return LangTypeEnum.Unknown;

            return languageScores.FirstOrDefault(x => x.Value == maxScore).Key;
        }

        /// <summary>
        /// 检查字符是否在指定范围内
        /// </summary>
        private static bool IsInRange(int codePoint, Tuple<int, int>[] ranges)
        {
            foreach (var range in ranges)
            {
                if (codePoint >= range.Item1 && codePoint <= range.Item2)
                    return true;
            }
            return false;
        }

        private static readonly Dictionary<LangTypeEnum, Tuple<int, int>[]> LanguageRanges = new Dictionary<LangTypeEnum, Tuple<int, int>[]>
        {
            // 中文：基本汉字和扩展汉字
            { LangTypeEnum.Chinese, new[] { Tuple.Create(0x4E00, 0x9FFF), Tuple.Create(0x3400, 0x4DBF) } },
        
            // 英文：基本拉丁字母
            { LangTypeEnum.English, new[] { Tuple.Create(0x0041, 0x007A) } },
        
            // 西班牙文：使用拉丁字母，与英文相同范围，但需要额外考虑特殊字符
            { LangTypeEnum.Spanish, new[] { Tuple.Create(0x0041, 0x007A), Tuple.Create(0x00C0, 0x00FF) } },
        
            // 法文：使用拉丁字母，与英文相同范围，但需要额外考虑特殊字符
            { LangTypeEnum.French, new[] { Tuple.Create(0x0041, 0x007A), Tuple.Create(0x00C0, 0x00FF) } },
        
            // 俄文：西里尔字母
            { LangTypeEnum.Russian, new[] { Tuple.Create(0x0400, 0x04FF) } },
        
            // 阿拉伯文：阿拉伯字母
            { LangTypeEnum.Arab, new[] { Tuple.Create(0x0600, 0x06FF), Tuple.Create(0x0750, 0x077F) } }
        };
    }
}

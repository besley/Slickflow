export class TaskAssistant {
    constructor() {
        this.nodeVectors = new Map();
        this.taskObjects = new Map(); 
    }

    initialize = async function (bpmnModeler) {
        // 获取注册表并查询
        const elementRegistry = bpmnModeler.get('elementRegistry');
        const allTasks = elementRegistry.getAll().filter(el => el.type.endsWith('Task'));
        allTasks.forEach(task => {
            const taskName = task.businessObject.name;
            this.nodeVectors.set(taskName, this.computeLightweightVector(taskName));
            this.taskObjects.set(taskName, task);
        })
    }

    // 新增轻量级向量计算函数
    computeLightweightVector = function (text) {
        // 使用词频统计 + 字符n-gram的混合方法
        const vector = new Array(128).fill(0); // 固定长度向量

        // 1. 字符级特征（ASCII值加权）
        for (let i = 0; i < text.length; i++) {
            const idx = i % vector.length;
            vector[idx] = (vector[idx] + text.charCodeAt(i) * 0.01) % 1;
        }

        // 2. 简单词频统计（无停用词过滤）
        const words = text.split(/\s+/);
        words.forEach(word => {
            if (word.length > 0) {
                const hash = this.stringHash(word) % vector.length;
                vector[hash] = (vector[hash] + 0.5) % 1;
            }
        });

        // 3. 添加长度特征
        vector[0] = Math.min(text.length / 50, 1); // 归一化长度

        return vector;
    }

    // 简易字符串哈希函数
    stringHash = function (str) {
        let hash = 0;
        for (let i = 0; i < str.length; i++) {
            hash = (hash << 5) - hash + str.charCodeAt(i);
            hash |= 0; // 转换为32位整数
        }
        return Math.abs(hash);
    }

    // 计算余弦相似度
    cosineSimilarity(vecA, vecB) {
        let dot = 0, magA = 0, magB = 0;
        for (let i = 0; i < vecA.length; i++) {
            dot += vecA[i] * vecB[i];
            magA += vecA[i] * vecA[i];
            magB += vecB[i] * vecB[i];
        }
        return dot / (Math.sqrt(magA) * Math.sqrt(magB));
    }

    // 查找最匹配的任务
    findMostSimilarTask(buttonText, threshold = 0.6) {
        const queryVector = this.computeLightweightVector(buttonText);
        let bestMatch = null;
        let highestSimilarity = -1;

        // 遍历所有任务向量
        for (const [taskName, taskVector] of this.nodeVectors) {
            const similarity = this.cosineSimilarity(queryVector, taskVector);

            if (similarity > highestSimilarity) {
                highestSimilarity = similarity;
                bestMatch = {
                    task: this.taskObjects.get(taskName),
                    similarity: similarity
                };
            }
        }

        // 返回相似度超过阈值的结果
        return highestSimilarity >= threshold ? bestMatch : null;
    }
}
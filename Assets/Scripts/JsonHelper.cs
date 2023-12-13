using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public static class JsonHelper
{

    public static T[] FromJson<T>(string json)
    {

        string dummy_json = $"{{\"{DummyNode<T>.ROOT_NAME}\": {json}}}";

        var obj = JsonUtility.FromJson<DummyNode<T>>(dummy_json);
        return obj.array;
    }


    public static string ToJson<T>(IEnumerable<T> collection)
    {
        string json = JsonUtility.ToJson(new DummyNode<T>(collection)); // �_�~�[���[�g���ƃV���A��������
        int start = DummyNode<T>.ROOT_NAME.Length + 4;
        int len = json.Length - start - 1;
        return json.Substring(start, len); // �ǉ����[�g�̕�������菜���ĕԂ�
    }

    // �����Ŏg�p����_�~�[�̃��[�g�v�f
    [System.Serializable]
    private struct DummyNode<T>
    {

        public const string ROOT_NAME = nameof(array);
        // �^���I�Ȏq�v�f
        public T[] array;
        // �R���N�V�����v�f���w�肵�ăI�u�W�F�N�g���쐬����
        public DummyNode(IEnumerable<T> collection) => this.array = collection.ToArray();
    }
}
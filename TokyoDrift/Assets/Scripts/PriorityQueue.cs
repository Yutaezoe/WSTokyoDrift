using System;
using System.Collections;
using System.Collections.Generic;

class PriorityQueue<T> where T : IComparable<T>
{
    private readonly T[] _array;
    private readonly IComparer _comparer;
    public int Count { get; private set; } = 0;
    public T Root => _array[0];

    public PriorityQueue(int capacity, IComparer comparer = null)
    {
        _array = new T[capacity];
        _comparer = comparer;
    }

    /// <summary>
    /// �v�f��}������
    /// </summary>
    public void Push(T item)
    {
        _array[this.Count] = item;
        Count += 1;

        var n = Count - 1;                  // ����(�ǉ�����)�̃m�[�h�̔ԍ�
        while (n != 0)
        {
            var parent = (n - 1) / 2;       // �e�m�[�h�̔ԍ�

            if (Compare(_array[n], _array[parent]) > 0)
            {
                Swap(n, parent);
                n = parent;
            }
            else
            {
                break;
            }
        }
    }

    /// <summary>
    /// �D��x�̈�ԍ������̂����o��
    /// </summary>
    public T Pop()
    {
        Swap(0, this.Count - 1);            // �擪�v�f�𖖔��ɂ���
        Count -= 1;

        var parent = 0;                     // �e�m�[�h�̔ԍ�
        while (true)
        {
            var child = 2 * parent + 1;     // �q�m�[�h�̔ԍ�
            if (child > Count - 1) break;

            // �l�̑傫�����̎q��I��
            if (child < Count - 1 && Compare(_array[child], _array[child + 1]) < 0) child += 1;

            // �q�̕����e���傫����Γ���ւ���
            if (Compare(_array[parent], _array[child]) < 0)
            {
                Swap(parent, child);
                parent = child;
            }
            else
            {
                break;
            }
        }

        return _array[Count];
    }

    /// <summary>
    /// �傫�����̂���񋓂��Ă���
    /// withPop = false�̂Ƃ��͏��Ԋ֌W�Ȃ����o�������Ȃ����f�����S�v�f���擾�ł���@
    /// </summary>
    public IEnumerable<T> GetAllElements(bool withPop = true)
    {
        int count = Count;
        for (int i = 0; i < count; i++)
        {
            if (withPop) yield return Pop();
            else yield return _array[count - i - 1];
        }
    }

    private int Compare(T a, T b)
    {
        if (_comparer == null) return a.CompareTo(b);
        return _comparer.Compare(a, b);
    }

    private void Swap(int a, int b)
    {
        var tmp = _array[a];
        _array[a] = _array[b];
        _array[b] = tmp;
    }
}
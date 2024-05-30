using System.Collections.Generic;

class PriorityQueue<T>
{
    private List<T> queueList = new();

    public void Enqueue(T element)
    {
        queueList.Add(element);
        queueList.Sort();
    }

    public T Dequeue()
    {
        if (queueList.Count == 0)
        {
            throw new System.ArgumentOutOfRangeException("You cannot Dequeue at empty Queue. Please check Queue Count.");
        }

        T returnValue = queueList[0];
        queueList.RemoveAt(0);
        return returnValue;
    }

    public int Count()
    {
        return queueList.Count;
    }

    public T Peek()
    {
        if (queueList.Count == 0)
        {
            throw new System.ArgumentOutOfRangeException("You cannot Peek at empty Queue. Please check Queue count.");
        }

        return queueList[0];
    }

    public List<T> InspectList()
    {
        return queueList;
    }
}
namespace Optimal.Framework.Util;

public static class TaskExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="task"></param>
    /// <returns></returns>
    public static T GetAsyncResult<T>(this Task<T> task)
    {
        return task.GetAwaiter().GetResult();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="task"></param>
    public static void GetAsyncResult(this Task task)
    {
        task.GetAwaiter().GetResult();
    }
}

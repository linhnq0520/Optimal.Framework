namespace Optimal.Framework.Extensions;

public static class TaskExtensions
{
    /// <summary>
    /// Get the result of a task
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="task"></param>
    /// <returns></returns>
    public static T GetAsyncResult<T>(this Task<T> task)
    {
        return task.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Get the result of a task
    /// </summary>
    /// <param name="task"></param>
    public static void GetAsyncResult(this Task task)
    {
        task.GetAwaiter().GetResult();
    }
}

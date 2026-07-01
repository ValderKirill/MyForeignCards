namespace MyForeignCards.Utils
{
    public class EndpiuntUtils
    {
        /// <summary>
        /// Возвращает часть пути эндпоинта по номеру
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string GetPathPart(PathString path, int number)
        {
            var pathSplit = path.Value?.Split("/");
            if (pathSplit != null && pathSplit.Count() >= number)
            {
                return pathSplit[number];
            }
            else
            {
                return string.Empty;
            }
        }
    }
}

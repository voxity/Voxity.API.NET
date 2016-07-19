
namespace Voxity.API
{
    /// <summary>
    /// Return Request response object.
    /// </summary>
    /// <example>
    /// Usage : <c>JsonConvert.DeserializeObject&lt;ApiResponse&lt;List&lt;Log>>>(JsonResponseString);</c>
    /// Deserialize JSON response string of <see cref="EndPoints.Calls.Logs(string, string, System.Collections.Specialized.StringCollection, System.Collections.Specialized.StringCollection, string, string)">call log</see> Request to a <see cref="Models.Log">log list</see> object.
    /// </example>
    /// <typeparam name="T">Request response API</typeparam>
    public class ApiResponse<T>
    {
        //uint status;

        /// <summary>
        /// 
        /// </summary>
        public T result;

        /// <summary>
        /// 
        /// </summary>
        public T data;

        /// <summary>
        /// 
        /// </summary>
        public T error;

        /// <summary>
        /// 
        /// </summary>
        public T Results()
        {
            if (this.result != null)
            {
                return this.result;
            }
            else if (data != null)
            {
                return this.data;
            }
            throw new ApiSessionException("No JSON object found in response request.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T Errors()
        {
            if (this.error != null)
            {
                return this.error;
            }
            throw new ApiSessionException("No JSON object found in response request.");
        }
    }
}

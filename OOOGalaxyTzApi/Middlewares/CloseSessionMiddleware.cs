using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ISession = NHibernate.ISession;

namespace OOOGalaxyTzApi.Middlewares
{
    /// <summary>
    /// Прослойка закрывающая сессию Nhibernate
    /// </summary>
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class CloseSessionMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Прослойка закрывающая сессию Nhibernate
        /// </summary>
        /// <param name="next"></param>
        public CloseSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Pipeline
        /// </summary>
        /// <param name="context"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, ISession session)
        {
            try
            {
                // Let the middleware pipeline run
                await _next(context);
            }
            finally
            {
                await CloseSession(session);
            }
        }

        private static async Task CloseSession(ISession session)
        {
            try
            {
                if (session.Transaction?.IsActive ?? false)
                    await session.Transaction.CommitAsync();
            }
            catch
            {
                if (session.Transaction?.IsActive ?? false)
                    await session.Transaction.RollbackAsync();

                throw;
            }
            finally
            {
                session.Dispose();
            }
        }
    }
}
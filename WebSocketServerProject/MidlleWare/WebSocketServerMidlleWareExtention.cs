namespace WebSocketServerProject.MidlleWare
{
    public static class WebSocketServerMidlleWareExtention
    {
        public static IApplicationBuilder UseWebSocketMiddleWare (this IApplicationBuilder builer)
        {
            return builer.UseMiddleware<WebSocketServerMidlleWare>();
        }

        public static IServiceCollection AddWebSocketMidlleWare(this IServiceCollection services)
        {
            services.AddSingleton<WebSocketServerConnectionManger> ();
            return services;
        }
    }
}

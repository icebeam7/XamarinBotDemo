using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Builder.Luis;

namespace BotAcercaDeLuis.Dialogs
{
    // primero app id, luego key
    [LuisModel("", "")]
    [Serializable]
    public class MeBotLuisDialog : LuisDialog<object>
    {
        public MeBotLuisDialog(params ILuisService[] services) : base(services)
        {
        }

        [LuisIntent("None")]
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Lo siento, mensaje no reconocido");
            context.Wait(MessageReceived);
        }

        [LuisIntent("AcercaDe")]
        public async Task AcercaDe(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"La búsqueda {result.Query} devolvió la siguiente información:");

            await context.PostAsync("Luis es un estudiante mexicano de doctorado en Rep. Checa");
            await context.PostAsync("Luis es un Microsoft MVP y XCMD");
            await context.PostAsync("Luis es un docente de ITC");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Busqueda")]
        public async Task Busqueda(IDialogContext context, LuisResult result)
        {
            string tag = string.Empty;
            string reply = string.Empty;
            List<Post> posts = new List<Post>();

            try
            {
                if (result.Entities.Count > 0)
                    tag = result.Entities.FirstOrDefault(e => e.Type == "Tag").Entity;

                if (!string.IsNullOrWhiteSpace(tag))
                    posts = await new Busqueda().BuscarPosts(tag.ToLower());

                foreach (var item in posts)
                {
                    await context.PostAsync($"{item.Title} - {item.Link}");
                }

                context.Wait(MessageReceived);
            }
            catch (Exception ex)
            {
                await context.PostAsync("Error");
                context.Wait(MessageReceived);
            }
        }
    }
}
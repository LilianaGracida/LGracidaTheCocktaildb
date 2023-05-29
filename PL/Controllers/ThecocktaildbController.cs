﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace PL.Controllers
{
    public class ThecocktaildbController : Controller
    {

        public ActionResult GetCocktail()
        {
            ML.Result resultLibro = new ML.Result();
                resultLibro.Objects = new List<Object>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://www.thecocktaildb.com/api/json/");

                    var responseTask = client.GetAsync("");
                    responseTask.Wait();

                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {

                        var readTask = result.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        foreach (var resultItem in readTask.Result.Objects)
                        {
                            ML.Cocktail resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cocktail>(resultItem.ToString());
                            resultLibro.Objects.Add(resultItemList);
                        }

                    }
                }
                ML.Cocktail cocktail = new ML.Cocktail();
                cocktail.Productos = resultLibro.Objects;
                return View(cocktail);//Mandar a llamar a la vista, mostrar la vista(HTML)
        }
        [HttpPost]
        public ActionResult GetCocktail(ML.Cocktail cocktail)
        {

            ML.Result resultProducto = new ML.Result();
            resultProducto.Objects = new List<Object>();

            string s = cocktail.Nombre;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://www.thecocktaildb.com/api/json/");

                var responseTask = client.GetAsync("v1/1/search.php?s=" + s);
                responseTask.Wait(); 

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();


                    foreach (var resultItem in readTask.Result.)
                    {
                        ML.Cocktail resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cocktail>(resultItem.ToString());

                        resultProducto.Objects.Add(resultItemList);
                    }

                }
            }

            return View(cocktail);
        }
    }
}
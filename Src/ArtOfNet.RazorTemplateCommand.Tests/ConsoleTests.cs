using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using ArtOfNet.RazorTemplateCommand.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace ArtOfNet.RazorTemplateCommand.Tests
{

    [TestFixture]
    public class ConsoleTests
    {
        [Test]
        public void ShouldGetGeneratedCodeFromTemplate()
        {
            string template1 = @"Hello @user, it's @Datetime.Now";
            string resultCode1 = RazorEngineHelper.GenerateSourceCode(template1);
            Assert.IsNotNullOrEmpty(resultCode1);
        }

        [Test]
        public void ShouldProcessSimpleTemplate()
        {

        }

        
        [Test]
        public void ShouldMapBetweenJsonAndDynamic()
        {
            dynamic model = new ExpandoObject();
            model.Speaker = "Rui";
            dynamic track1 = new ExpandoObject();
            track1.Title = "MongoDB en C#";
            track1.Date = new DateTime(2011, 2, 8, 17, 30, 0);
            dynamic track2 = new ExpandoObject();
            track2.Title = "Asp.Net MVC3";
            track2.Date = new DateTime(2011, 2, 9, 17, 30, 0);
            model.Sessions = new[] { track1, track2 };
            string json = JsonConvert.SerializeObject(model, Formatting.Indented);
            dynamic actual = JsonConvert.DeserializeObject<ExpandoObject>(json);

            Assert.IsNotNull(actual.Speaker);
            Assert.IsNotNull(actual.Sessions);
            Assert.IsInstanceOf(typeof(ICollection<Newtonsoft.Json.Linq.JToken>), actual.Sessions);
            Assert.AreEqual(2,actual.Sessions.Count);
            
        }

        [Test]
        public void ShouldUseJsonAsModelForTemplate()
        {
            dynamic modelSource = new ExpandoObject();
            modelSource.Speaker = "Rui";
            dynamic track1 = new ExpandoObject();
            track1.Title = "MongoDB en C#";
            track1.Date = new DateTime(2011, 2, 8, 17, 30, 0);
            dynamic track2 = new ExpandoObject();
            track2.Title = "Asp.Net MVC3";
            track2.Date = new DateTime(2011, 2, 9, 17, 30, 0);
            modelSource.Sessions = new[] { track1, track2 };
            string json = JsonConvert.SerializeObject(modelSource, Formatting.Indented);
            dynamic actualModel = JsonConvert.DeserializeObject<ExpandoObject>(json);

            string templateSource= @"Session presented by @Model.Speaker, Total Sessions : @Model.Sessions.Count";

            string actual = RazorEngineHelper.ProcessGeneratedCode(templateSource, actualModel);
            string expected = "Session presented by Rui, Total Sessions : 2";
            Assert.IsNotNullOrEmpty(actual);
            Assert.AreEqual(expected, actual);
        }

    }
}

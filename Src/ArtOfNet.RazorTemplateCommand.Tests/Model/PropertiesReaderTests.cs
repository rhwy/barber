using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ArtOfNet.RazorTemplateCommand.Framework;
using System.Dynamic;

namespace ArtOfNet.RazorTemplateCommand.Tests.Model
{
    [TestFixture]
    public class PropertiesReaderTests
    {
        private string source = @"
        [AppConfig]
        key1=value1
        key2 = value2
        #key3 = removed
        [section2]
        
        key3=
        key4=aslist1
        key4=aslist2
        key4=isitreallyalist?
        ";

        private string serializedSource = @"'\r\n[User]\r\nName=Rui\r\n\r\n[Children]\r\nThais=girl\r\nLeandre=boy\r\n'";

        [Test]
        public void ShouldGetSanitizedTitle()
        {
            PropertiesReader reader = new PropertiesReader(source);
            string badFormedTitle = "a 0°==eori";
            string sanitized = reader.SanitizeSectionTitle(badFormedTitle);
            string expected = "a0eori";
            Assert.AreEqual(expected,sanitized);
        }

        [Test]
        public void TestPropertiesParsing()
        {
            PropertiesReader reader = new PropertiesReader(source);

            dynamic properties = reader.GetProperties();
            Assert.IsNotNull(properties);
            Assert.AreEqual(2,properties.Keys.Count);
            Assert.IsNotNull(properties.AppConfig);
            Assert.AreEqual(2,properties.AppConfig.Keys.Count);
            Assert.IsNotNull(properties.section2);
            Assert.AreEqual(2,properties.section2.Keys.Count);
            Assert.AreEqual(string.Empty,properties.section2.key3);
            Assert.IsInstanceOf<List<string>>(properties.section2.key4);
            Assert.AreEqual(3,properties.section2.key4.Count);
        }

        [Test]
        public void TestSerializedPropertiesParsing()
        {
            PropertiesReader reader = new PropertiesReader(serializedSource);
            dynamic properties = reader.GetProperties();
            
            Assert.IsNotNull(properties);
            
        }

    }
}

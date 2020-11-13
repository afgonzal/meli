using System.Linq;
using ML.FichaTecnica.BusinessEntities;
using Moq;
using NUnit.Framework;

namespace ML.FichaTecnica.Services.Tests
{
    public class Tests
    {
        private Mock<IMeliClient> _meliClient;

        [SetUp]
        public void Setup()
        {
            _meliClient = new Mock<IMeliClient>();
            _meliClient.Setup(x => x.GetItem(It.IsAny<string>())).ReturnsAsync(MockedItem());
            _meliClient.Setup(x => x.GetTechnicalSpecs(It.IsAny<string>())).ReturnsAsync(MockedTechSpec());
        }

        private TechnicalSpecs MockedTechSpec()
        {
            return new TechnicalSpecs
            {
                MainTitle = "MyTechSpec", Groups = Enumerable.Range(1, 5).Select(id => new Group
                {
                    Id = $"Group{id}", Label = $"Grp{id}",
                    Section = "SomeSection", Components = Enumerable.Range(1, 2).Select(cid => new Component
                        {
                            ComponentType = "TEXT_OUTPUT",
                            Label = $"CompLbl{cid}",
                            Attributes = Enumerable.Range(1, 4).Select(aid => new Attribute
                                {Id = $"attr{id}", Name = $"AttrName{id}", ValueName = $"AttrVName{id}"}).ToList()
                        }
                    ).ToList()
                }).ToList()
            };
        }

        private Item MockedItem()
        {
            return new Item
            {
                Id = "MLA1234", DomainId = "MLA-CASAS", CategoryId = "Alq", Title = "Casa linda",
                Attributes = Enumerable.Range(1, 10).Select(id => new Attribute
                    {Id = $"attr{id}", Name = $"AttrName{id}", ValueName = $"AttrVName{id}"}).ToList()
            };
        }

        [Test]
        public void Success_Ok()
        {

            Assert.Pass();
        }
    }
}
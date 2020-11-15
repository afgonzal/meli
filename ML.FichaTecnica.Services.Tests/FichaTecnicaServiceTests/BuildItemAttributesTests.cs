using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ML.FichaTecnica.BusinessEntities;
using Moq;
using NUnit.Framework;

namespace ML.FichaTecnica.Services.Tests
{
    public class Tests
    {
        private Mock<IMeliClient> _meliClient;
        private Mock<ILogger<FichaTecnicaService>> _logger;
        private Mock<INumbersToLanguage> _numbersService;

        [SetUp]
        public void Setup()
        {
            _meliClient = new Mock<IMeliClient>();
         
            _logger = new Mock<ILogger<FichaTecnicaService>>();
            _numbersService = new Mock<INumbersToLanguage>();
            _numbersService.Setup(x => x.Int2Espanol(It.IsAny<string>())).Returns("Cero");
        }

        private TechnicalSpecs MockedTechSpec()
        {
            return new TechnicalSpecs
            {
                MainTitle = "MyTechSpec", Groups = Enumerable.Range(1, 5).Select(id => new Group
                {
                    Id = $"Group{id}", Label = $"Grp{id}",
                    Section = "SomeSection", Components = Enumerable.Range(0, 5).Select(
                        cid => new Component
                        {
                            Label = $"Label {(ComponentTypes) cid}", ComponentType = ((ComponentTypes) cid).ToString(),
                            Attributes = Enumerable.Range(1, 2).Select(aid => new ComponentAttribute
                                {Id = $"attr{(id-1)*10+cid*2+aid}", Name = $"name{aid}"}).ToList()
                        }
                    ).ToList()
                }).ToList()
            };
        }

        private ComponentAttribute MockAttribute(int index)
        {
            //manera de matchear matriz de group components generada anteriormente, y que haya items de todos los output 
            index = index * 4 + 2;
            var componentType = (ComponentTypes) ((index-(Math.Ceiling((decimal)index / 10)-1)*10)/2-1);

            switch (componentType)
            {
                case ComponentTypes.IGNORE:
                case ComponentTypes.TEXT_OUTPUT:
                case ComponentTypes.BOOLEAN_OUTPUT:
                    return new ComponentAttribute
                    {
                        Id = $"attr{index}",
                        Name = $"Attribute {index}",
                        ValueName = componentType.ToString()
                    };
                case ComponentTypes.NUMBER_OUTPUT:
                    return new ComponentAttribute
                    {
                        Id = $"attr{index}",
                        Name = $"Attribute {index}",
                        ValueName = index.ToString()
                    };
                case ComponentTypes.NUMBER_UNIT_OUTPUT:
                    return new ComponentAttribute
                    {
                        Id = $"attr{index}",
                        Name = $"Attribute {index}",
                        ValueStruct = new ValueStruct { Number = index, Unit = "Hz" }
                    };
            }
            return null;
        }

        private Item MockedItem()
        {
            return new Item
            {
                Id = "MLA1234", DomainId = "MLA-CASAS", CategoryId = "Alq", Title = "Casa linda",
                Attributes = Enumerable.Range(1, 10).Select(MockAttribute).ToList()
            };
        }

        [Test]
        public async Task Success_Ok()
        {
            _meliClient.Setup(x => x.GetItem(It.IsAny<string>())).ReturnsAsync(MockedItem());
            _meliClient.Setup(x => x.GetTechnicalSpecs(It.IsAny<string>())).ReturnsAsync(MockedTechSpec());

            var service = new FichaTecnicaService(_meliClient.Object, _logger.Object, _numbersService.Object);
            var result = await service.BuildItemAttributes("MLA34");

            Assert.NotNull(result);
            Assert.IsInstanceOf<ItemAttributesOutput>(result);
            Assert.IsNotEmpty(result.Title);
            Assert.IsNotEmpty(result.Id);
            Assert.True(result.Groups.Any());
            Assert.AreEqual(10, result.Groups.Sum(g => g.Components.Count));

            _meliClient.Verify(x => x.GetItem(It.IsAny<string>()), Times.Once);
            _meliClient.Verify(x => x.GetTechnicalSpecs(It.IsAny<string>()), Times.Once);
            _logger.Verify(x => x.Log(It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Exactly(2));
        }

        [Test]
        public async Task Success_ItemHasNoAttributes_ReturnEmptyList()
        {
            var noAttributes = MockedItem();
            noAttributes.Attributes = new List<ComponentAttribute>();
            _meliClient.Setup(x => x.GetItem(It.IsAny<string>())).ReturnsAsync(noAttributes);
            _meliClient.Setup(x => x.GetTechnicalSpecs(It.IsAny<string>())).ReturnsAsync(MockedTechSpec());

            var service = new FichaTecnicaService(_meliClient.Object, _logger.Object, _numbersService.Object);
            var result = await service.BuildItemAttributes("MLA34");

            Assert.NotNull(result);
            Assert.IsInstanceOf<ItemAttributesOutput>(result);
            Assert.IsNotEmpty(result.Title);
            Assert.IsNotEmpty(result.Id);
            Assert.False(result.Groups.Any());

            _meliClient.Verify(x => x.GetItem(It.IsAny<string>()), Times.Once);
            _meliClient.Verify(x => x.GetTechnicalSpecs(It.IsAny<string>()), Times.Never);
            _logger.Verify(x => x.Log(It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }
        [Test]
        public async Task Success_FTHasNoGroups_ReturnEmptyList()
        {
            var noGroups = MockedTechSpec();
            noGroups.Groups = new List<Group>();
            _meliClient.Setup(x => x.GetItem(It.IsAny<string>())).ReturnsAsync(MockedItem());
            _meliClient.Setup(x => x.GetTechnicalSpecs(It.IsAny<string>())).ReturnsAsync(noGroups);

            var service = new FichaTecnicaService(_meliClient.Object, _logger.Object, _numbersService.Object);
            var result = await service.BuildItemAttributes("MLA34");

            Assert.NotNull(result);
            Assert.IsInstanceOf<ItemAttributesOutput>(result);
            Assert.IsNotEmpty(result.Title);
            Assert.IsNotEmpty(result.Id);
            Assert.False(result.Groups.Any());

            _meliClient.Verify(x => x.GetItem(It.IsAny<string>()), Times.Once);
            _meliClient.Verify(x => x.GetTechnicalSpecs(It.IsAny<string>()), Times.Once);
            _logger.Verify(x => x.Log(It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }

        [Test]
        public async Task Fail_GetFTReturnsNull_ThrowException()
        {
            _meliClient.Setup(x => x.GetItem(It.IsAny<string>())).ReturnsAsync((Item)null);
            _meliClient.Setup(x => x.GetTechnicalSpecs(It.IsAny<string>())).ReturnsAsync(MockedTechSpec());

            var service = new FichaTecnicaService(_meliClient.Object, _logger.Object, _numbersService.Object);
            Assert.ThrowsAsync<ArgumentException>(async () => await service.BuildItemAttributes("MLA34"));


            _meliClient.Verify(x => x.GetItem(It.IsAny<string>()), Times.Once);
            _meliClient.Verify(x => x.GetTechnicalSpecs(It.IsAny<string>()), Times.Never);
            _logger.Verify(x => x.Log(It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }

        [Test]
        public async Task Fail_GetItemReturnsNull_ThrowException()
        {
            _meliClient.Setup(x => x.GetItem(It.IsAny<string>())).ReturnsAsync(MockedItem());
            _meliClient.Setup(x => x.GetTechnicalSpecs(It.IsAny<string>())).ReturnsAsync((TechnicalSpecs)null);

            var service = new FichaTecnicaService(_meliClient.Object, _logger.Object, _numbersService.Object);
            Assert.ThrowsAsync<ArgumentException>(async () => await service.BuildItemAttributes("MLA34"));


            _meliClient.Verify(x => x.GetItem(It.IsAny<string>()), Times.Once);
            _meliClient.Verify(x => x.GetTechnicalSpecs(It.IsAny<string>()), Times.Once);
            _logger.Verify(x => x.Log(It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }



        [Test]
        public async Task Fail_GetItemFails_ThrowException()
        {
            _meliClient.Setup(x => x.GetItem(It.IsAny<string>())).ThrowsAsync(new HttpRequestException());
            _meliClient.Setup(x => x.GetTechnicalSpecs(It.IsAny<string>())).ReturnsAsync(MockedTechSpec());

            var service = new FichaTecnicaService(_meliClient.Object, _logger.Object, _numbersService.Object);
            Assert.ThrowsAsync<HttpRequestException>(async () => await service.BuildItemAttributes("MLA34"));

            
            _meliClient.Verify(x => x.GetItem(It.IsAny<string>()), Times.Once);
            _meliClient.Verify(x => x.GetTechnicalSpecs(It.IsAny<string>()), Times.Never);
            _logger.Verify(x => x.Log(It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }

        [Test]
        public async Task Fail_GetFT_ThrowException()
        {
            _meliClient.Setup(x => x.GetItem(It.IsAny<string>())).ReturnsAsync(MockedItem());
            _meliClient.Setup(x => x.GetTechnicalSpecs(It.IsAny<string>())).ThrowsAsync(new HttpRequestException());

            var service = new FichaTecnicaService(_meliClient.Object, _logger.Object, _numbersService.Object);
            Assert.ThrowsAsync<HttpRequestException>(async () => await service.BuildItemAttributes("MLA34"));


            _meliClient.Verify(x => x.GetItem(It.IsAny<string>()), Times.Once);
            _meliClient.Verify(x => x.GetTechnicalSpecs(It.IsAny<string>()), Times.Once);
            _logger.Verify(x => x.Log(It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }
    }
}
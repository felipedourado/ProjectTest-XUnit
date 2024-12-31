using Bogus;
using JornadaMilhasV1.Gerencidor;
using JornadaMilhasV1.Modelos;

namespace JornadaMilhas.Test;

//Bogus
//Geração de Dados Aleatórios: Bogus fornece uma variedade de geradores para criar dados falsos, incluindo nomes, endereços, números de telefone, e-mails, números de cartão de crédito, entre outros. Isso permite simular uma ampla gama de situações de teste com facilidade.
//Configuração Flexível: Você pode personalizar os dados gerados de acordo com suas necessidades.A biblioteca oferece muitas opções de configuração para controlar o formato e o tipo de dados gerados, permitindo que você crie conjuntos de dados específicos para seus testes.//Suporte a Internacionalização: Bogus suporta geração de dados em vários idiomas e formatos.Isso é útil para testar aplicativos que têm requisitos de localização e internacionalização.
//Integração com Frameworks de Teste: A biblioteca é facilmente integrada com frameworks de teste populares, como o xUnit.Isso simplifica a incorporação de dados falsos em seus casos de teste existentes.
//https://github.com/bchavez/Bogus
public class GerenciadorDeOfertasDesconto
{
    [Fact]
    public void RetornaOfertaNulaQuandoListaEstaVazia()
    {
        //arrange
        var lista = new List<OfertaViagem>();
        var gerenciador = new GerenciadorDeOfertas(lista);
        Func<OfertaViagem, bool> filtro = d => d.Rota.Destino.Equals("São Paulo");

        //act
        var oferta = gerenciador.RecuperaMaiorDesconto(filtro);

        //assert
        Assert.Null(oferta);
    }

    [Fact]
    // destino = são paulo, desconto = 40, preco = 80
    public void RetornaOfertaEspecificaQuandoDestinoSaoPauloEDesconto40()
    {
        //arrange
        var fakerPeriodo = new Faker<Periodo>().CustomInstantiator(f =>
        {
            DateTime dataInicio = f.Date.Soon();
            return new Periodo(dataInicio, dataInicio.AddDays(30));
        });

        var rota = new Rota("Curitiba", "São Paulo");

        //bogus
        var fakerOferta = new Faker<OfertaViagem>()
            .CustomInstantiator(f => new OfertaViagem(rota, fakerPeriodo.Generate(),
        100 * f.Random.Int(1, 100)))
            .RuleFor(o => o.Desconto, f => 40)
            .RuleFor(o => o.Ativa, f => true);

        var ofertaEscolhida = new OfertaViagem(rota, fakerPeriodo.Generate(), 80)
        {
            Desconto = 40,
            Ativa = true
        };

        var ofertaInativa = new OfertaViagem(rota, fakerPeriodo.Generate(), 70)
        {
            Desconto = 40,
            Ativa = false
        };

        var lista = fakerOferta.Generate(200);
        lista.Add(ofertaEscolhida);
        lista.Add(ofertaInativa);
        var gerenciador = new GerenciadorDeOfertas(lista);
        Func<OfertaViagem, bool> filtro = o => o.Rota.Destino.Equals("São Paulo");
        var precoEsperado = 40;

        //act
        var oferta = gerenciador.RecuperaMaiorDesconto(filtro);

        //assert
        Assert.NotNull(oferta);
        Assert.Equal(precoEsperado, oferta.Preco, 0.0001);
    }
}

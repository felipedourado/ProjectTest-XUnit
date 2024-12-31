using JornadaMilhasV1.Modelos;

namespace JornadaMilhas.Test;

#region Leia-me Padrão de Teste de Unidade e Como Planejar Teste de Unidade 
//padrão utilizado AAA, existe o padrão GWT (Give-When-Then é mais orientado ao
//comportamento e coloca mais ênfase na descrição do comportamento do sistema em termos
//de entradas e saídas) 


//nomenclatura dos metodos de testes
//Legibilidade: utilizar nomes claros e descritivos facilita a compreensão do propósito de cada teste;
//Manutenção: padrões consistentes tornam a manutenção do código de teste mais fácil;
//Comunicação: ter padrões de nomenclatura ajuda na comunicação entre os membros da equipe;
//Documentação implícita: nomes consistentes e descritivos podem servir como documentação implícita do código de teste;
//Padronização da equipe: aplicar padrões de nomenclatura promovem consistência dentro da equipe de desenvolvimento.
//https://ardalis.com/mastering-unit-tests-dotnet-best-practices-naming-conventions/
//https://www.alura.com.br/artigos/tipos-de-testes-principais-por-que-utiliza-los
//faltam testes: teste de carga, teste de automação, teste de mutação, teste de segurança


//Como planejar o teste de unidade
//Identificação do caso de teste - cada caso de teste deve ter um identificador único, que pode ser um nome descritivo, por exemplo;
//Descrição - uma descrição concisa do que o caso de teste está testando;
//Pré-condições - as condições ou estados que devem ser verdadeiros antes que o caso de teste possa ser executado com sucesso;
//Entradas - os dados de entrada necessários para executar o caso de teste;
//Passos de execução - uma lista detalhada das etapas que devem ser seguidas para executar o caso de teste;
//Resultados esperados - os resultados específicos que são esperados após a execução bem-sucedida do caso de teste;
//Critérios de aceitação - critérios claros que determinam se o caso de teste foi aprovado ou reprovado.


//info sobre o xunit
//[TestFixture] - uma classe que contém um conjunto de testes de unidade relacionados;
//[Test] - utilizada para identificar testes distintos dentro de uma mesma classe de teste;
//[Ignore] - utilizada para ignorar um teste específico durante a execução;
//[Collection] - utilizada para agrupar testes em coleções específicas.
//[Theory] - Testa a mesma ação para cenários diferentes.

#endregion

public class OfertaViagemConstrutor
{
    [Fact]
    public void RetornaOfertaValidaQuandoDadosValidos()
    {
        //arrange
        var rota = new Rota("origem", "destino");
        var periodo = new Periodo(new DateTime(2024, 2, 10), DateTime.Now);
        double preco = 100.0;
        var validacao = true;

        //act
        var oferta = new OfertaViagem(rota, periodo, preco);

        //assert
        Assert.Equal(validacao, oferta.EhValido);
    }

    [Fact]
    public void RetornaMensagemDeErroDeRotaOuPeriodoInvalidosQuandoRotaNula()
    {
        //arrange
        var periodo = new Periodo(new DateTime(2024, 2, 10), DateTime.Now);
        double preco = 100.0;

        //act
        var oferta = new OfertaViagem(null!, periodo, preco);

        //assert
        Assert.Contains("A oferta de viagem não possui rota ou período válidos.",
            oferta.Erros.Sumario);

        Assert.False(oferta.EhValido);
    }

    [Fact]
    public void RetornaMensagemDeErroDePeriodoInvalidoQuandoPeriodoDataDeIdaMaiorQueDataFinal()
    {
        //arrange
        Rota rota = new("OrigemTeste", "DestinoTeste");
        Periodo periodo = new(new DateTime(2024, 2, 5), new DateTime(2024, 2, 1));
        double preco = 100.0;

        //act
        OfertaViagem oferta = new OfertaViagem(rota, periodo, preco);

        //assert
        Assert.Contains("Erro: Data de ida não pode ser maior que a data de volta.", oferta.Erros.Sumario);
        Assert.False(oferta.EhValido);
    }

    [Fact]
    public void RetornaMensagemDeErroDePrecoInvalidoQuandoPrecoMenorQueZero()
    {
        //arrange
        Rota rota = new("OrigemTeste", "DestinoTeste");
        Periodo periodo = new(new DateTime(2024, 2, 10), DateTime.Now);
        double preco = -100.0;

        //act
        OfertaViagem oferta = new(rota, periodo, preco);

        //assert
        Assert.Contains("O preço da oferta de viagem deve ser maior que zero.", oferta.Erros.Sumario);
        Assert.False(oferta.EhValido);
    }

    [Theory]
    [InlineData("", null, "2024-01-01", "2024-01-02", 0, false)]
    [InlineData("OrigemTeste", "DestinoTeste", "2024-02-01", "2024-02-05", 100, true)]
    [InlineData(null, "São Paulo", "2024-01-01", "2024-01-02", -1, false)]
    [InlineData("Vitória", "São Paulo", "2024-01-01", "2024-01-01", 0, false)]
    [InlineData("Rio de Janeiro", "São Paulo", "2024-01-01", "2024-01-02", -500, false)]
    public void RetornaEhValidoDeAcordoComDadosDeEntrada(string origem, string destino,
        string dataIda, string dataVolta, double preco, bool validacao)
    {
        //arrange
        var rota = new Rota(origem, destino);
        Periodo periodo = new(DateTime.Parse(dataIda), DateTime.Parse(dataVolta));

        //act
        var oferta = new OfertaViagem(rota, periodo, preco);

        //assert
        Assert.Equal(validacao, oferta.EhValido);
    }


    [Fact]
    public void RetornaTresErrosDeValidacaoQuandoRotaPeriodoEPrecoSaoInvalidos()
    {
        //arrange
        int quantidadeEsperada = 3;
        Rota rota = null;
        Periodo periodo = new Periodo(new DateTime(2024, 6, 1), new DateTime(2024, 5, 10));
        double preco = -100;

        //act
        OfertaViagem oferta = new(rota, periodo, preco);

        //assert
        Assert.Equal(quantidadeEsperada, oferta.Erros.Count());
    }
}
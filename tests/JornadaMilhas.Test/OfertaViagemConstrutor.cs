using JornadaMilhasV1.Modelos;

namespace JornadaMilhas.Test;

#region Leia-me Padr�o de Teste de Unidade e Como Planejar Teste de Unidade 
//padr�o utilizado AAA, existe o padr�o GWT (Give-When-Then � mais orientado ao
//comportamento e coloca mais �nfase na descri��o do comportamento do sistema em termos
//de entradas e sa�das) 


//nomenclatura dos metodos de testes
//Legibilidade: utilizar nomes claros e descritivos facilita a compreens�o do prop�sito de cada teste;
//Manuten��o: padr�es consistentes tornam a manuten��o do c�digo de teste mais f�cil;
//Comunica��o: ter padr�es de nomenclatura ajuda na comunica��o entre os membros da equipe;
//Documenta��o impl�cita: nomes consistentes e descritivos podem servir como documenta��o impl�cita do c�digo de teste;
//Padroniza��o da equipe: aplicar padr�es de nomenclatura promovem consist�ncia dentro da equipe de desenvolvimento.
//https://ardalis.com/mastering-unit-tests-dotnet-best-practices-naming-conventions/
//https://www.alura.com.br/artigos/tipos-de-testes-principais-por-que-utiliza-los
//faltam testes: teste de carga, teste de automa��o, teste de muta��o, teste de seguran�a


//Como planejar o teste de unidade
//Identifica��o do caso de teste - cada caso de teste deve ter um identificador �nico, que pode ser um nome descritivo, por exemplo;
//Descri��o - uma descri��o concisa do que o caso de teste est� testando;
//Pr�-condi��es - as condi��es ou estados que devem ser verdadeiros antes que o caso de teste possa ser executado com sucesso;
//Entradas - os dados de entrada necess�rios para executar o caso de teste;
//Passos de execu��o - uma lista detalhada das etapas que devem ser seguidas para executar o caso de teste;
//Resultados esperados - os resultados espec�ficos que s�o esperados ap�s a execu��o bem-sucedida do caso de teste;
//Crit�rios de aceita��o - crit�rios claros que determinam se o caso de teste foi aprovado ou reprovado.


//info sobre o xunit
//[TestFixture] - uma classe que cont�m um conjunto de testes de unidade relacionados;
//[Test] - utilizada para identificar testes distintos dentro de uma mesma classe de teste;
//[Ignore] - utilizada para ignorar um teste espec�fico durante a execu��o;
//[Collection] - utilizada para agrupar testes em cole��es espec�ficas.
//[Theory] - Testa a mesma a��o para cen�rios diferentes.

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
        Assert.Contains("A oferta de viagem n�o possui rota ou per�odo v�lidos.",
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
        Assert.Contains("Erro: Data de ida n�o pode ser maior que a data de volta.", oferta.Erros.Sumario);
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
        Assert.Contains("O pre�o da oferta de viagem deve ser maior que zero.", oferta.Erros.Sumario);
        Assert.False(oferta.EhValido);
    }

    [Theory]
    [InlineData("", null, "2024-01-01", "2024-01-02", 0, false)]
    [InlineData("OrigemTeste", "DestinoTeste", "2024-02-01", "2024-02-05", 100, true)]
    [InlineData(null, "S�o Paulo", "2024-01-01", "2024-01-02", -1, false)]
    [InlineData("Vit�ria", "S�o Paulo", "2024-01-01", "2024-01-01", 0, false)]
    [InlineData("Rio de Janeiro", "S�o Paulo", "2024-01-01", "2024-01-02", -500, false)]
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
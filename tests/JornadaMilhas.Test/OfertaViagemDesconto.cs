using JornadaMilhasV1.Modelos;

namespace JornadaMilhas.Test;

//Fluxo do TDD (Test-Driven Development - Red - Green- Refactor)
//Nova funcionalidade, escrever o teste primeiro
//ajustar onde está ocorrendo erro no codigo para fazer o teste funcionar
//refatorar o codigo
//Red (Vermelho): Nesta fase, você escreve um teste automatizado que captura uma pequena parte do comportamento desejado do sistema. No entanto, o código de produção ainda não foi implementado, então o teste deve falhar (ficar "vermelho").
//Green(Verde) : Agora, o objetivo é fazer com que o teste escrito na fase anterior passe.Você implementa apenas o código necessário para que o teste automatizado tenha êxito.O foco é escrever o código mínimo necessário para atender aos requisitos do teste.
//Refactor(Refatorar): Com o teste passando, você pode refatorar o código para melhorar sua qualidade, eficiência e legibilidade.O objetivo é garantir que o código continue atendendo aos requisitos, mas agora de uma maneira mais clara e eficiente.
//Ganhos:
//Feedback rápido: Os testes automatizados fornecem feedback instantâneo sobre a funcionalidade implementada.
//Código mais seguro e robusto: Como cada alteração é validada por testes automatizados, o risco de introdução de erros é reduzido.
//Documentação viva: Os testes automatizados servem como documentação atualizada e viva do comportamento do sistema.
//Facilita a refatoração: A abordagem de refatoração contínua ajuda a melhorar a qualidade do código ao longo do tempo.

//testes de mutação (utilizar o comando dotnet stryker e abrir o relatorio no navegador)
//Testes de mutação são uma técnica avançada de teste de software que visa avaliar a eficácia dos testes de unidade identificando lacunas na cobertura do código. Os testes de mutação são particularmente úteis para garantir que os testes não apenas verifiquem a implementação atual do código, mas também sejam robustos o suficiente para detectar mudanças semânticas significativas que possam introduzir bugs.
//Os testes de mutação seguem o seguinte fluxo ao serem aplicados:
//Introdução de mutações: Um processo automatizado é usado para introduzir pequenas alterações no código-fonte, conhecidas como mutações.
//Execução dos testes: Depois que as mutações são introduzidas no código-fonte, os testes de unidade existentes são executados novamente.Se um teste de unidade falhar após a introdução de uma mutação, isso indica que o teste conseguiu detectar a mudança no comportamento do código.
//Análise dos resultados: Os resultados dos testes de mutação são analisados para determinar a eficácia dos testes existentes.Se um grande número de mutações não são detectadas pelos testes, isso sugere que há lacunas na cobertura de teste e que os testes podem não ser robustos o suficiente para detectar todas as variações no comportamento do código.
//Refinamento dos testes: Com base nos resultados da análise, as pessoas desenvolvedoras podem refinar os testes de unidade existentes ou adicionar novos testes para melhorar a cobertura e garantir que o código seja mais robusto contra alterações.

public class OfertaViagemDesconto
{
    [Fact]
    public void RetornaPrecoAtualizadoQuandoAplicaDesconto()
    {
        //arrange
        var rota = new Rota("origem", "destino");
        var periodo = new Periodo(new DateTime(2024, 2, 10), DateTime.Now);
        double preco = 100.0;
        double desconto = 20.00;
        double precoComDesconto = preco - desconto;

        //act
        OfertaViagem oferta = new(rota, periodo, preco)
        {

            Desconto = desconto
        };

        //assert
        Assert.Equal(precoComDesconto, oferta.Preco);
    }

    [Fact]
    public void RetornaDescontoMaximoQuandoValorDescontoMaiorQuePreco()
    {
        //arrange
        var rota = new Rota("origem", "destino");
        var periodo = new Periodo(new DateTime(2024, 2, 10), DateTime.Now);
        double preco = 100.0;
        double desconto = 120.00;
        double precoComDesconto = 30;

        //act
        OfertaViagem oferta = new(rota, periodo, preco)
        {

            Desconto = desconto
        };

        //assert
        Assert.Equal(precoComDesconto, oferta.Preco, 0.001);
    }

    [Fact]
    public void RetornaPrecoQuandoValorDescontoNegativo()
    {
        //arrange
        var rota = new Rota("origem", "destino");
        var periodo = new Periodo(new DateTime(2024, 2, 10), DateTime.Now);
        double preco = 100.0;
        double desconto = -100.00;
        double precoComDesconto = preco;

        //act
        OfertaViagem oferta = new(rota, periodo, preco)
        {

            Desconto = desconto
        };

        //assert
        Assert.Equal(precoComDesconto, oferta.Preco);
    }

    [Theory]
    [InlineData(120, 30)]
    [InlineData(100, 30)]
    [InlineData(0, 100)]
    public void RetornaDescontoMaximoQuandoValorDescontoMaiorOuIgualAoPreco(double desconto, double precoComDesconto)
    {
        //arrange
        var rota = new Rota("origem", "destino");
        var periodo = new Periodo(new DateTime(2024, 2, 10), DateTime.Now);
        double preco = 100.0;

        //act
        OfertaViagem oferta = new(rota, periodo, preco)
        {

            Desconto = desconto
        };

        //assert
        Assert.Equal(precoComDesconto, oferta.Preco, 0.001);
    }

}

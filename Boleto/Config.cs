namespace WebApplication2.Boleto
{
    public class Config
    {

        public FormFile RetornaObjetoFormFile(string filePath)
        {
            var stream = new MemoryStream(File.ReadAllBytes(filePath).ToArray());

            var formFile = new FormFile(stream,
                                               0,
                                               stream.Length,
                                               Path.GetFileName(filePath),
                                               filePath.Split(@"\").Last());

            return formFile;
        }

        
        public string ini = $@"[Cedente] 

                        Nome=SAO JOAO LTDA.

                        CNPJCPF= 84.504.412/0001-59

                        Logradouro=Rua Evaristo Mendes

                        Numero=200

                        Bairro=Centro

                        Cidade=Tatui

                        CEP=18.270-000

                        Complemento=Sala 10

                        UF=SP

                        RespEmis=0

                        TipoPessoa=1

                        CodigoCedente=123456

                        LayoutBol=3

                        CaracTitulo=0

                        TipoCarteira=0

                        TipoDocumento=0

                        Modalidade=17

                        CodTransmissao=10

                        Convenio=123456

                        PIX.TipoChavePix=1

                        PIX.Chave=teste@testechave.com



                        [Conta]

                        Conta=99999

                        DigitoConta=9

                        Agencia=9999

                        DigitoAgencia=9

                        DigitoVerificadorAgenciaConta=



                        [Banco]

                        Numero=237

                        CNAB=1

                        TipoCobranca=5

                        NumeroCorrespondente=0

                        VersaoArquivo=0

                        VersaoLote=0



                        [Titulo1]
                        NumeroDocumento=000001
                        SeuNumero=000001
                        NossoNumero=12345
                        Carteira=17
                        ValorDocumento=100,00
                        Vencimento=08/05/2025
                        DataDocumento=08/04/2025
                        DataProcessamento=08/04/2025
                        DataDesconto=28/04/2025
                        TipoDesconto=0
                        ValorDesconto=0,50
                        CodigoMora=1
                        ValorMoraJuros=0,20
                        DataMoraJuros=08/05/2025
                        ValorIOF=0
                        ValorOutrasDespesas=2,50
                        DataMulta=08/05/2025
                        MultaValorFixo=1
                        PercentualMulta=5,00
                        DiasDeProtesto=0
                        DataProtesto=07/06/2025
                        TipoDiasProtesto=0
                        CodigoNegativacao=0
                        TipoDiasNegativacao=0
                        Especie=DM
                        EspecieMod=R$
                        Aceite=1
                        Instrucao1=10
                        Instrucao2=11
                        TipoImpressao=0
                        CarteiraEnvio=0

                        [Titulo1.Sacado]
                        NomeSacado= Jose
                        Pessoa=0
                        CNPJCPF=84.504.412/0001-59
                        Logradouro=Rua da Colina
                        Numero=1111
                        Bairro=Centro
                        Complemento=Prédio 2
                        Cidade=Tatui
                        UF=SP
                        CEP=18280-000
                        Email=josesilva@mail.com

                        [Titulo1.Sacado.Avalista]
                        NomeAvalista=Sociedade Consultoria
                        Pessoa=1
                        CNPJCPF=84.504.412/0001-59
                        Logradouro=Rua Frei Caneca
                        Numero=100
                        Complemento=Predio 2
                        Bairro=Centro
                        Cidade=Sao Paulo
                        UF=SP
                        CEP=18280000
                        Email=sconsultoria@mail.com
                        InscricaoNr=99999999999
                                                ";


    }
}

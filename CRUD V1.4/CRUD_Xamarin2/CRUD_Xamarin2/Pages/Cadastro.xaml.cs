using App8.Email;
using CRUD_Xamarin2.Administrador;
using CRUD_Xamarin2.CRUD;
using CRUD_Xamarin2.Funcionalidades;
using CRUD_Xamarin2.ModeloUsuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRUD_Xamarin2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Cadastro : ContentPage
    {
        //inicialização das variaveis auxiliares

        PadraoADM adm = new PadraoADM();
        CPF validador = new CPF();
        Usuario usuario = new Usuario();
        ModeloBanco banco = new ModeloBanco();
        ServicoEmail mail = new ServicoEmail();
        bool _chamadoPelaAdm;

        public Cadastro(bool chamadoPelaAdm)
        {
            InitializeComponent();
            adm.Adm = false;
            _chamadoPelaAdm = chamadoPelaAdm;
        }

        private async void voltarPagina(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void entrarPagina()
        {
            await Navigation.PushModalAsync(new Exibir(adm, usuario, false));
        }


        //funçao para cadastrar o usuario
        private void cadastrar(object sender, EventArgs e)
        {
            //Verificação de operações

            //Espaço Vazio
            if (string.IsNullOrWhiteSpace(lbl_cpf.Text))
            {
                DisplayAlert("Aviso!", "Por - favor, insira um CPF válido!", "OK");
            }

            //Validar CPF
            else if (!validador.IsValid(lbl_cpf.Text))
            {
                DisplayAlert("Aviso!", "Por - favor, insira um CPF válido!", "OK");
            }

            //Espaço Vazio
            else if (string.IsNullOrWhiteSpace(lbl_nome.Text))
            {
                DisplayAlert("Aviso!", "Preencha o campo E - MAIL", "Entendi!");
            }

            //Email sem '@' e '.'
            else if (lbl_nome.Text.IndexOf('@')==-1|| lbl_nome.Text.IndexOf('.')==-1)
            {
                DisplayAlert("Aviso!", "Insira um EMAIL VÁLIDO!", "Entendi!");
            }

            //Espaço Vazio
            else if (string.IsNullOrWhiteSpace(lbl_senha.Text))
            {
                DisplayAlert("Aviso!", "Preencha o campo SENHA", "Entendi!");
            }

            //Senha Diferente de Copiar Senha
            else if (lbl_senha.Text != lbl_senha_confirmar.Text)
            {
                DisplayAlert("Aviso!", "As senhas NÃO SÃO IGUAIS!", "Entendi!");
            }
            else
            {
                //atribui os campos para o usuario auxiliar
                usuario.Adm = false;
                usuario.Nome = lbl_nome.Text;
                usuario.Senha = lbl_senha.Text;
                usuario.Cpf = lbl_cpf.Text;
                adm.Adm = usuario.Adm;

                //verifica se ja existe no banco de dados
                var quantidade = banco.GetUsuario(usuario);

                if (quantidade.Count == 0)
                {
                    //insere no banco se nao houver registro igual
                    if (banco.InserirUsuario(usuario))
                    {

                        DisplayAlert("Parabéns!", "Cadastrado com sucesso!", "OK");
                        //apos cadastrar envia um e mail de boas vindas para o usuario
                        mail.EnviarEmail(lbl_nome.Text);
                        //ve se essa pagina foi chamada por um admnistrador ou pela tela normal de login
                        if (!_chamadoPelaAdm)
                        {
                            //ve se o usuario é admnistrador e chama a funçao de entrar na pagina
                            adm.Adm = usuario.Adm;
                            entrarPagina();
                        }
                        else
                        {
                            //se for pelo admnistrador volta para a pagina do adm
                            adm.Adm = usuario.Adm;
                            voltarPaginaNoAdiconarComAdmnistrador();
                        }
                        
                    }
                    else
                    {
                        DisplayAlert("Aviso!", "Já existe uma conta cadastrada com o CPF informado!", "Entendi!");
                    }
                }
                else
                {
                    DisplayAlert("Aviso!", "Já existe uma conta cadastrada com o CPF informado!", "Entendi!");
                }
            }
        }

        //função para voltar para a pagina do adm
        private async void voltarPaginaNoAdiconarComAdmnistrador()
        {
            await Navigation.PopModalAsync();
        }
    }
}

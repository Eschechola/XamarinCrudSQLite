using CRUD_Xamarin2.CRUD;
using CRUD_Xamarin2.ModeloUsuarios;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRUD_Xamarin2.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AdicionarOuRemoverADM : PopupPage
	{
        //objetos e variaveis auxiliares
        bool _noturno;
        ModeloBanco banco = new ModeloBanco();
		public AdicionarOuRemoverADM (bool noturno)
		{
			InitializeComponent ();
            _noturno = noturno;
		}

        //Remoção ou Inserção de Usuario Adm, sem permitir a remoção do ADM Base
        private async void adicionarOuRemoverAdm(object sender, EventArgs e)
        {
            //objetos e variaveis auxiliares
            Usuario user = new Usuario();
            //cpf para pesquisar no banco
            user.Cpf = lbl_pesquisar.Text;
            //pesquisa o usuario no banco
            var listaComUsuario = banco.GetUsuarioSemSenha(user);

            //se o cpf for do admnistrador raiz, nao permite o login
            if (user.Cpf == "000.000.000-00")
            {
                DisplayAlert("Erro", "Você não pode alterar o ADMNISTRADOR PADRÃO!", "Entendi");
            }
            else if (listaComUsuario.Count < 1)
            {
                //se o usuario nao for encontrado pelo cpf
                DisplayAlert("Aviso!", "Usuário não encontrado!", "Ok");
            }
            else
            {
                //caso ele ja tenha adm ele vai retirar o adm da pessoa
                if (listaComUsuario[0].Adm)
                {
                    var certeza = await DisplayAlert("Confirmar", "Você tem certeza que deseja RETIRAR O ADMNISTRADOR DE "+listaComUsuario[0].Cpf, "Sim","Não");
                    if (certeza)
                    {
                        //atualiza o usuario auxiliar
                        user.Adm = false;
                        var sucesso = banco.AtualizarAdmin(user);
                        if (sucesso)
                        {
                            DisplayAlert("Sucesso", "Agora " + user.Cpf + " não é um admnistrador!", "Entendi!");
                            PopupNavigation.RemovePageAsync(this);
                        }
                    }
                }
                else
                {
                    //caso ele nao tenha admn ele vai adicionar adm para a pessa
                    var certeza = await DisplayAlert("Confirmar", "Você tem certeza que deseja ADICIONAR PERMISSÃO DE ADMNISTRADOR EM " + listaComUsuario[0].Cpf, "Sim", "Não");
                    if (certeza)
                    {
                        //atualiza o objeto auxiliar
                        user.Adm = true;
                        var sucesso = banco.AtualizarAdmin(user);
                        if (sucesso)
                        {
                            DisplayAlert("Sucesso", "Agora " + user.Cpf + " é um admnistrador!", "Entendi!");
                            PopupNavigation.RemovePageAsync(this);
                        }
                    }
                }
                
            }
        }
    }
}
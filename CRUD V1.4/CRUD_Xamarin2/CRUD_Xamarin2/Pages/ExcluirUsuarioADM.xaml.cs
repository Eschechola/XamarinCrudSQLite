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
	public partial class ExcluirUsuarioADM : PopupPage
	{
        //variaveis e objetos auxiliares
        bool _noturno;
        ModeloBanco banco = new ModeloBanco();

		public ExcluirUsuarioADM (bool noturno)
		{
			InitializeComponent ();
            _noturno = noturno;
		}

        //função pra pesquisar o usuario
        private void pesquisarUsuarioParaAlterar(object sender, EventArgs e)
        {
            //usuario auxiliar pra pesquisar
            Usuario user = new Usuario();
            user.Cpf = lbl_pesquisar.Text;
            //pesquisa o usuario
            var listaComUsuario = banco.GetUsuarioSemSenha(user);
            int chamarErro2 = 0;
            var sucesso = false;
            
            //se nao achar o usuario exibe uma mensagem
            if (listaComUsuario.Count < 1)
            {
                DisplayAlert("Erro", "Algo deu errado! Por - favor, tente - novamente.", "Entendi");
            }
            //se o tentar excluir o usuario padrao
            else if (user.Cpf == "000.000.000-00")
            {
                DisplayAlert("Erro", "Você não pode deletar o ADMNISTRADOR PADRÃO!", "Entendi");
                chamarErro2++;
            }
            else
            {
                //senao ele pega o usuario e tenta deletar no banco
                user = listaComUsuario[0];
                sucesso = banco.DeletarUsuario(user);
            }


            //caso delete ele exibe uma mensagem
            if (sucesso)
            {
                DisplayAlert("Parabéns!", user.Cpf + " foi deletado com sucesso!", "Ok");
                PopupNavigation.RemovePageAsync(this);
            }
            else
            {
                //erro se tentar deletar o adm
                if (chamarErro2 == 0)
                {
                    DisplayAlert("Erro!", "Não foi possível excluir o usuário! Por - favor, tente novamente!", "Entendi");
                }
            }
        }
    }
}
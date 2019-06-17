using CRUD_Xamarin2.CRUD;
using CRUD_Xamarin2.ModeloUsuarios;
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
	public partial class PaginaAdmnistrador : ContentPage
	{
        ModeloBanco banco = new ModeloBanco();
        Cadastro cadastro;
        bool _noturno;

		public PaginaAdmnistrador (bool noturno)
		{
			InitializeComponent ();
            _noturno = noturno;
            var listaComTodosOsUsuarios = banco.GetTodosUsuarios();
            listaTodosUsuarios.ItemsSource = listaComTodosOsUsuarios;
		}

        private void atualizarUsuarios(object sender, EventArgs e)
        {
            var listaComTodosOsUsuarios = banco.GetTodosUsuarios();
            listaTodosUsuarios.ItemsSource = listaComTodosOsUsuarios;
            listaTodosUsuarios.IsRefreshing = false;
        }

        private async void tornarUsuarioAdmninstrador(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var user = menuItem.CommandParameter as Usuario;
            var listaComUsuario = banco.GetUsuarioSemSenha(user);
            if (user.Cpf == "000.000.000-00")
            {
                DisplayAlert("Erro", "Você não pode alterar o ADMNISTRADOR PADRÃO!", "Entendi");
            }
            else if (listaComUsuario.Count < 1)
            {
                DisplayAlert("Aviso!", "Usuário não encontrado!", "Ok");
            }
            else
            {
                if (listaComUsuario[0].Adm)
                {
                    var certeza = await DisplayAlert("Confirmar", "Você tem certeza que deseja RETIRAR O ADMNISTRADOR DE " + listaComUsuario[0].Cpf, "Sim", "Não");
                    if (certeza)
                    {
                        user.Adm = false;
                        var sucesso = banco.AtualizarAdmin(user);
                        if (sucesso)
                        {
                            DisplayAlert("Sucesso", "Agora " + user.Cpf + " não é um admnistrador!", "Entendi!");
                        }
                    }
                }
                else
                {
                    var certeza = await DisplayAlert("Confirmar", "Você tem certeza que deseja ADICIONAR PERMISSÃO DE ADMNISTRADOR EM " + listaComUsuario[0].Cpf, "Sim", "Não");
                    if (certeza)
                    {
                        user.Adm = true;
                        var sucesso = banco.AtualizarAdmin(user);
                        if (sucesso)
                        {
                            DisplayAlert("Sucesso", "Agora " + user.Cpf + " é um admnistrador!", "Entendi!");
                        }
                    }
                }

            }
        }

        private void excluirDadosUsuario(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var pessoa = menuItem.CommandParameter as Usuario;
            int chamarErro2 = 0;
            var sucesso = false;
            if (pessoa.Cpf == "000.000.000-00")
            {
                DisplayAlert("Erro", "Você não pode deletar o ADMNISTRADOR PADRÃO!", "Entendi");
                chamarErro2++;
            }
            else
            {
                 sucesso = banco.DeletarUsuario(pessoa);
            }
            
            if (sucesso)
            {
                var listaComTodosOsUsuarios = banco.GetTodosUsuarios();
                listaTodosUsuarios.ItemsSource = listaComTodosOsUsuarios;
                DisplayAlert("Parabéns!", pessoa.Cpf+" foi deletado com sucesso!", "Ok");
            }
            else
            {
                if (chamarErro2 == 0)
                {
                    DisplayAlert("Erro!", "Não foi possível excluir o usuário! Por - favor, tente novamente!", "Entendi");
                }
            }
        }

        private void alterarDadosUsuario(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var user = menuItem.CommandParameter as Usuario;
            chamarPaginaAlterarUsuario(user);


        }

        private async void chamarPaginaAlterarUsuario(Usuario user)
        {
            await Navigation.PushModalAsync(new AlterarUsuario(user, _noturno));
        }

        private void adicionarUsuario(object sender, EventArgs e)
        {
            cadastro = new Cadastro(true);
            chamarPaginaDeCadastro();
        }

        private async void chamarPaginaDeCadastro()
        {
            await Navigation.PushModalAsync(cadastro);
        }

        private async void alterarUsuario(object sender, EventArgs e)
        {
            PesquisarUsuarioADM pagina;
            pagina = new PesquisarUsuarioADM(_noturno);
            await PopupNavigation.PushAsync(pagina);
        }

        private async void adicionarOuRemoverAdmin(object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new AdicionarOuRemoverADM(_noturno));
        }

        private async void abrirDeletarUsuario(object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new ExcluirUsuarioADM(_noturno));
        }
    }
}
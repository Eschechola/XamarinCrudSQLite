using CRUD_Xamarin2.Administrador;
using CRUD_Xamarin2.CRUD;
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
	public partial class Inicio : ContentPage
	{
        //carrega a pagina de login
        Login login = new Login();
        //carrega a pagina de cadastro
        Cadastro cadastro = new Cadastro(false);
        //instancia a classe com as funçoes de banco
        ModeloBanco banco = new ModeloBanco();
        //cria um usuario para verificar se tem alguma conta salva
        Usuario user = new Usuario();
        //classe para definir se o usuario é admnistrador ou n
        PadraoADM adm = new PadraoADM();

        public Inicio ()
		{
			InitializeComponent ();
            //cria o banco de dados onde vai ficar a conta salva
            banco.CriarBancoDeDadosLembrar();
            //ve se tem algum usuario gravado para ser lembrado
            var pegarUsuarioSalvo = banco.GetUsuarioSalvar();
            //se o usuario salvo for difirente de 000.000.000-00 (ADM PADRAO)
            //habilita o botao de entrar com a conta salva
            if (pegarUsuarioSalvo[0].Cpf != "000.000.000-00")
            {
                btn_salvo.IsEnabled = true;
                btn_salvo.IsVisible = true;
            }
            
        }

        private async void Login_Click(object sender, EventArgs e)
        {
            //chama a pagina de login
            await Navigation.PushModalAsync(login);
        }

        private async void Cadastro_Click(object sender, EventArgs e)
        {
            //chama a pagina de cadastro
            await Navigation.PushModalAsync(cadastro);
        }

        private void entrarComSenhaSalva(object sender, EventArgs e)
        {
            //define o adm como falso
            adm.Adm = false;
            //pega o usuario que ja foi salvo para ser lembrado
            var pegarUsuarioSalvo = banco.GetUsuarioSalvar();
            //define o cpf do usuario auxiliar como o cpf do usuario salvo
            user.Cpf = pegarUsuarioSalvo[0].Cpf;
            DisplayAlert("Aviso!", "Voce logou com uma conta que estava salva para ser lembrada.\n\nPara desabilitar isso, vá em configurações e clique em SAIR!", "Ok");
            //pega todos os dados do usuario atraves do cpf dele
            var pegarUsuarioDoBanco = banco.GetUsuarioSemSenha(user);
            //define que o usuario auxiliar é igual ao usuario pego
            user = pegarUsuarioDoBanco[0];
            //se o usuario pego for diferente do admnistrador, chama a pagina
            if (pegarUsuarioSalvo[0].Cpf != "000.000.000-00")
            {
                Navigation.PushModalAsync(new Exibir(adm, user, false));
            }
            
        }
    }
}
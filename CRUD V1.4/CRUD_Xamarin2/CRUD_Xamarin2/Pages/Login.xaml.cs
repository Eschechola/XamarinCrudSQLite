using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD_Xamarin2.Administrador;
using CRUD_Xamarin2.CRUD;
using CRUD_Xamarin2.Funcionalidades;
using CRUD_Xamarin2.ModeloUsuarios;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRUD_Xamarin2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : ContentPage
	{
        //inicializando objetos auxiliares
        PadraoADM adm = new PadraoADM();
        Usuario usuario = new Usuario();
        public Login ()
		{
			InitializeComponent ();
            //passando adm como false
            adm.Adm = false;
		}

        //funçao pra chamar a  tela de esqueceu a senha
        private async void Esqueci_Click(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Esq_Senha());
        }


        //funçao para verificar campos e fazer login
        private async void EntrarList(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(lbl_cpf.Text))
            {
                await DisplayAlert("Aviso!", "Insira um CPF", "Ok");
            }
            else if (string.IsNullOrWhiteSpace(lbl_cpf.Text))
            {
                await DisplayAlert("Aviso!", "Insira uma SENHA", "Ok");
            }
            else
            {
                //inicializa a criptografia
                Criptografia criptar = new Criptografia();
                usuario.Cpf = lbl_cpf.Text;
                //senha do usuario auxiliar cripografada
                usuario.Senha = criptar.Encriptar(lbl_senha.Text);
                //incializa o objeto para o crud
                ModeloBanco banco = new ModeloBanco();
                //ve se o usuario existe no banco
                var achouUsuario = banco.GetUsuario(usuario);

                if (achouUsuario.Count < 1)
                {
                    var op = await DisplayActionSheet("Usuário não encontrado!\nPor favor tente novamente ou:", "Ok,", null, "Cadastre - se");
                    if(op=="Cadastre - se")
                    {
                        Navigation.PushModalAsync(new Cadastro(false));
                    }
                }
                else
                {
                    //se existir ele loga como usuario
                    usuario = achouUsuario[0];
                    adm.Adm = usuario.Adm;

                    if (senhaSalva.IsChecked)
                    {
                        //se o senha salva estiver habilitado ele tenta salvar a senha do usuario
                        var trocarSenhaSalva = banco.AlterarSenhaSalva(usuario, true);
                        if (!trocarSenhaSalva)
                        {
                            DisplayAlert("Erro!", "Não foi possível lembrar sua senha!\n Por favor, tente novamente!", "Ok");
                        }
                    }
                    await Navigation.PushModalAsync(new Exibir(adm, usuario, false));
                }
            }
            
        }
    }
}
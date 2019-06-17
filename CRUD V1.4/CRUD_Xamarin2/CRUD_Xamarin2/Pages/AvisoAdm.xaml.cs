using CRUD_Xamarin2.Administrador;
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

namespace CRUD_Xamarin2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AvisoAdm : PopupPage
	{
        //variaveis auxiliares
        Usuario _user;
        PadraoADM _adm;
        bool _noturno;
		public AvisoAdm (Usuario user, PadraoADM adm, bool noturno)
		{
			InitializeComponent ();
            _user = user;
            _adm = adm;
            _noturno = noturno;
            //Modo Noturno
            if (noturno)
            {
                stk1.BackgroundColor = Color.DimGray;
                lbl1.TextColor = Color.White;
                lbl2.TextColor = Color.White;
                btn1.TextColor = Color.White;
                btn1.BackgroundColor = Color.Gray;
                btn2.TextColor = Color.White;
                btn2.BackgroundColor = Color.Gray;
            }
		}

        //funçao para retirar o modal
        private async void voltarExibir(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync();
        }

        //função para chamar a pagina de admnistrador
        private async void abrirConfirguracoes(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Configuracoes(_user, _adm, _noturno));
            await PopupNavigation.PopAsync();
        }
    }
}
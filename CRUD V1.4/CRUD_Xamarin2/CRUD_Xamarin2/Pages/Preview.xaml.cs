using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD_Xamarin2.Administrador;
using CRUD_Xamarin2.CRUD;
using CRUD_Xamarin2.ModeloUsuarios;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRUD_Xamarin2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Preview : ContentPage
    {
        //instanciando as classe banco para criar os bancos
        ModeloBanco banco = new ModeloBanco();
        //carrega ja pagina inicio pra ser chamada 
        Inicio carregarInicio = new Inicio();
        public Preview()
        {
            InitializeComponent();
            //cria o banco de dados dos filmes
            banco.CriarBancoDeDadosFilmes();
            //cria o banco de dados dos usuarios
            banco.CriarBancoDeDadosUsuarios();
            //cria o banco de dados onde vai ficar a senha pra ser lembrada
            banco.CriarBancoDeDadosLembrar();
            ChamarInicio();
        }

        private async void ChamarInicio()
        {
            //gera uma tarefa de 3,5 segundos
            await Task.Delay(3500);
            //fala que a main page foi a pagina ja carregada no começo
            App.Current.MainPage = carregarInicio;
        }
    }
}
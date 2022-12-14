using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using web.Entities.Pessoa.Model;
using web.Entities.Pessoa.DAO;
using web.Entities.Factory;
using web.Entities.PException;
using System.Threading.Tasks;


namespace web.Pages
{
    public class CadastrarModel : PageModel
    {
        public PessoaInfo pessoa = new PessoaInfo();
        private PessoaDAO dao;
        public string errorMessage = "";

        public void OnGet()
        {

        }

        public void OnPost()
        {
            try
            {
                pessoa.SetCpf(Request.Form["cpf"]);
                pessoa.SetNome(Request.Form["nome"]);
                pessoa.SetProfissao(Request.Form["profissao"]);
                pessoa.SetNacionalidade(Request.Form["nacionalidade"]);
                pessoa.SetDataNascimento(DateTime.Parse(Request.Form["dataNascimento"]));
                pessoa.SetPeso(float.Parse(Request.Form["peso"]));
                pessoa.SetAltura(float.Parse(Request.Form["altura"]));
            }
            catch (FormatException erro)
            {
                this.errorMessage = "Todos os campos devem ser preenchidos corretamente.";
                return;
            }
            catch (Exception erro)
            {
                this.errorMessage = erro.Message;
                return;
            }

            try
            {
                if (pessoa.ValidaCPF())
                {
                    errorMessage = "CPF inv?lido.";
                    return;
                }
            }
            catch (ArgumentOutOfRangeException erro)
            {
                this.errorMessage = "O CPF precisa ser preenchido com valores num?ricos.";
                return;
            }
            catch (Exception erro)
            {
                errorMessage = erro.Message;
                return;
            }

            if (pessoa.DataMaior())
            {
                this.errorMessage = "A data de nascimento n?o pode ser maior que a data atual";
                return;
            }

            if (pessoa.LengthCampos() || pessoa.LengthCpf())
            {
                this.errorMessage = "Algum campo n?o foi preenchido ou excedeu seu limite de caracteres. Tente Novamente.";
                return;
            }

            if (pessoa.GetDataNascimentoUS() == "0001-01-01")
            {
                this.errorMessage = "Preencha a data de nascimento corretamente.";
                return;
            }


            // salvando dados no banco caso n?o tenha erros
            try
            {
                dao = new PessoaDAO();
                dao.InsertInto(pessoa);
            }
            catch (Exception erro)
            {
                this.errorMessage = erro.Message;
                return;
            }

            Response.Redirect("/Listagem");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using web.Entities.Pessoa.Model;
using web.Entities.Pessoa.DAO;
using web.Entities.PException;


namespace web.Pages
{
    public class EditarModel : PageModel
    {
        private PessoaDAO dao = new PessoaDAO();
        public PessoaInfo pessoa = new PessoaInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            string cpf = Request.Query["cpf"];

            try
            {
                pessoa = dao.SelectCPF(cpf);
            }
            catch (Exception erro)
            {
                this.errorMessage = erro.Message;
            }
        }

        public void OnPost()
        {
            try
            {
                pessoa.SetCpf(Request.Query["cpf"]);
                pessoa.SetNome(Request.Form["nome"]);
                pessoa.SetProfissao(Request.Form["profissao"]);
                pessoa.SetNacionalidade(Request.Form["nacionalidade"]);
                pessoa.SetDataNascimento(DateTime.Parse(Request.Form["dataNascimento"]));
                try
                {
                    pessoa.SetPeso(float.Parse(Request.Form["peso"]));
                    pessoa.SetAltura(float.Parse(Request.Form["altura"]));
                }
                catch (FormatException erro)
                {
                    this.errorMessage = "� necess�rio digitar um valor num�rico separados por V�RGULA para os campos de Peso e Altura.";
                    return;
                }
            }
            catch (FormatException erro)
            {
                this.errorMessage = "Todos os campos devem ser preenchidos.";
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
                    errorMessage = "CPF Inv�lido";
                    return;
                }
            }
            catch (ArgumentOutOfRangeException erro)
            {
                this.errorMessage = "O CPF precisa ser preenchido com valores num�ricos.";
                return;
            }
            catch (Exception erro)
            {
                errorMessage = erro.Message;
                return;
            }

            if (pessoa.DataMaior())
            {
                this.errorMessage = "A data de nascimento n�o pode ser maior que a data atual";
                return;
            }

            if (pessoa.LengthCampos())
            {
                this.errorMessage = "Algum campo n�o foi preenchido ou excedeu seu limite de caracteres. Tente Novamente.";
                return;
            }

            // salvando dados caso n�o haja erro
            try
            {
                dao.Update(pessoa);
            }
            catch (Exception erro)
            {
                this.errorMessage = erro.Message;
            }

            Response.Redirect("/Listagem");
        }
    }
}

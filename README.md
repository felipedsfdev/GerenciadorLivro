# 📚 Gerenciador de Livros – API da Biblioteca

Projeto desenvolvido como parte da avaliação FIAP, utilizando **.NET 8** e **SQLite**.  
Inclui modelagem do domínio, regras de negócio, validações e documentação básica.

---

## 🧱 Entidades

### **📘 Livro**
- ISBN (único)  
- Título  
- Autor  
- Categoria  
- Status (DISPONIVEL, EMPRESTADO, RESERVADO)  
- DataCadastro  

### **👤 Usuário**
- Id  
- Nome  
- Email  
- Tipo (ALUNO, PROFESSOR, FUNCIONARIO)  
- DataCadastro  

### **📕 Empréstimo**
- Id  
- ISBNLivro  
- UsuarioId  
- DataEmprestimo  
- DataPrevistaDevolucao  
- DataRealDevolucao  
- Status (ATIVO, FINALIZADO, ATRASADO)  

### **💰 Multa**
- EmprestimoId  
- Valor  
- Status (PENDENTE, PAGA)  

---

## ⚙️ Regras de Negócio Implementadas

- Usuários podem ter **no máximo 3 empréstimos ativos**.  
- Livros emprestados **não podem ser emprestados novamente**.  
- Professores possuem **prazo maior para devolução**.  
- Multas calculadas automaticamente: **R$ 1,00 por dia de atraso**.  
- Usuários com **multa pendente não podem realizar novos empréstimos**.  
- Devolução só é permitida para empréstimos com **status ATIVO**.

---

## 🚀 Como Executar

Para colocar a API em funcionamento, siga os passos abaixo:

### 1️⃣ Entre na pasta **Api**

Abra seu terminal ou prompt de comando e navegue até o diretório do projeto onde está o arquivo da API:

- cd Api
- dotnet run
- Acesse localhost:5291/swagger
- E teste suas requisições!!

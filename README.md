<div class="markdown-heading" dir="auto">
    <h1 tabindex="-1" class="heading-element" dir="auto">Store E-commerce Enterprise</h1> 
    <h4> RabbitMQ, gRPC, Docker, NGINX (ssl) </h4>
</div>
  <p dir="auto">O objetivo é criar uma aplicação <em>e-commerce</em> voltada para a área corporativa usando boas práticas e estratégias levando em consideração escalabilidade, segurança, modelagem do negócio/software e afins.</p>

<hr>

<div class="markdown-heading" dir="auto">
  <h2 tabindex="-1" class="heading-element" dir="auto">Visão geral da arquitetura adotada</h2>
</div>

<p dir="auto">A arquitetura proposta para esta aplicação não serve de modelo ou template para qualquer outra. 
  Assim cada aplicação deve ter uma arquitetura específica de acordo com o cenário e nuances do problema do negócio. 
  Assim, a arquitetura da aplicação é diretamente proporcional a complexidade do negócio.</p>
  
<p dir="auto">Dado o contexto, a solução arquitetural adotada para esta aplicação é a <strong>distribuída</strong>, 
    ou também comumente chamada de <strong>monolíto distribuído</strong>. Uma das suas principais características é a divisão da aplicação por 
    <a href="https://www.eduardopires.net.br/2016/03/ddd-bounded-context/" rel="nofollow">
      contextos delimitados</a>, consequentemente distribuindo a aplicação em serviços responsáveis, onde cada um cuida de um subdomínio do negócio. 
    A imagem abaixo demonstra a visão geral da arquitetura, contendo 6 contextos delimitados (subdomínios), que são: 
    <ul dir="auto">
      <strong>
        <li> Autenticação (Store.Authentication.API) </li> 
        <li> Cliente (Store.Customer.API) </li>
        <li> Catalogo (Store.Catalog.API) </li>
        <li> Carrinho (Store.ShopCart.API) </li>
        <li> Pedido (Store.Orders.API)</li>
        <li> Pagamento (Store.Payment.API) Modelo Gateway de Pagamento (fake) </li>
      </strong></p>
    </ul>
  
  <p dir="auto"><a target="_blank" rel="noopener noreferrer" href="https://github.com/raphamendes123/e-commerce/blob/main/docs/arquitetura-aplicacao.png">
  <img src="https://github.com/raphamendes123/e-commerce/blob/main/docs/arquitetura-aplicacao.png" alt="Arquitetura da aplicação" style="max-width: 100%;"></a></p>

  <p dir="auto">Esta aplicação <strong>não possui uma arquitetura em microserviços, e sim baseada em microserviços</strong>. Isto se dá porque ela abre mão de "regras" que são necessárias em uma aplicação em microserviços. Além disso, aplicações nesse estilo arquitetural são complexas e quase sempre desnecessárias para o contexto do problema de negócio que se deseja resolver, como é citado pelo Eduardo Pires:</p>
<blockquote>

  <p dir="auto"><em>"Usar aplicações em microservices são para poucos cenários, onde são necessários extremamente de atualizações e escalabilidade (a cada pouco tempo tem que subir novidades tendo milhares/milhões de usuários), como por exemplo em aplicações como: Netflix, Uber, Amazon e afins. Agora em cenários de aplicações corporativas em qualquer tipo de setor comercial/industrial pode ser muito bem entregue no modelo com arquitetura distribuída. E caso evolua muito a aplicação este modelo está a poucos passos de se tornar um microserviço."</em></p>
</blockquote>

  <p dir="auto">Dessa forma, utilizarmos uma arquitetura em microserviços dependendo da complexidade do negócio pode ser uma <em>"bazuca para matar uma barata"</em>.</p><hr>

  <div class="markdown-heading" dir="auto"><h2 tabindex="-1" class="heading-element" dir="auto">Como executar?</h2>
    <a id="user-content-como-executar" class="anchor" aria-label="Permalink: Como executar?" href="#como-executar">
      <svg class="octicon octicon-link" viewBox="0 0 16 16" version="1.1" width="16" height="16" aria-hidden="true" />
    </a>
  </div>

<p dir="auto">Existem duas maneiras principais de execução da aplicação: a partir do 
  <a href="https://visualstudio.microsoft.com/pt-br/downloads/" rel="nofollow">Visual Studio</a>, ou usando 
  <a href="https://www.docker.com/" rel="nofollow">Docker</a>. Também pode ser usado 
  <a href="https://code.visualstudio.com/download" rel="nofollow">Visual Code</a>, mas os dois citados anteriormente são mais práticos.
</p>

<div class="markdown-heading" dir="auto">
  <h3 tabindex="-1" class="heading-element" dir="auto">Execução pelo Visual Studio</h3>
  <a id="user-content-execução-pelo-visual-studio" class="anchor" aria-label="Permalink: Execução pelo Visual Studio" href="#execução-pelo-visual-studio" />
</div>

<p dir="auto">Para executar no Visual Studio, basta seguir os seguinte passos:</p>
  <ul dir="auto">
    <li>
      <p dir="auto">Instalar as seguintes ferramentas: .Net 8, SQL Server (preferencialmente com o SSMS) e Docker;</p>
    </li>
    <li>
      <p dir="auto">Criar uma instância do RabbitMQ a partir de um container docker por meio do seguinte comando:</p>
      <div class="snippet-clipboard-content notranslate position-relative overflow-auto"><pre class="notranslate">
      <code>
        docker run -d --hostname rabbit-host --name rabbit-ecommerce -p 15672:15672 -p 5672:5672 rabbitmq:management
      </code>
</pre>
  <div class="zeroclipboard-container">
    <clipboard-copy aria-label="Copy" class="ClipboardButton btn btn-invisible js-clipboard-copy m-2 p-0 d-flex flex-justify-center flex-items-center" data-copy-feedback="Copied!" data-tooltip-direction="w" value="docker run -d --hostname rabbit-host --name rabbit-nerdstore -p 15672:15672 -p 5672:5672 rabbitmq:3-management-alpine" tabindex="0" role="button">
    </clipboard-copy>
  </div></div>
</li>
<li>
<p dir="auto">
  Para criação da base de dados existem duas maneiras. Uma delas é executar os scripts da pasta SQL no banco de dados. Outra forma é rodar as <em>migrations</em> presentes no código fonte em cada um dos projetos através do <a href="https://www.learnentityframeworkcore.com/migrations/commands/pmc-commands" rel="nofollow"><em>PMC (Package Manager Console)</em></a> do Visual Studio. Caso escolha a segunda maneira basta rodar o seguinte comando em cada projeto no <em>PMC</em>:</p>
<div class="snippet-clipboard-content notranslate position-relative overflow-auto"><pre class="notranslate">
  <code> Update-Database -Context {nome_contexto} -StartupProject {nome_projeto_startup} 
  </code>
</pre>
<div class="zeroclipboard-container">
    <clipboard-copy aria-label="Copy" class="ClipboardButton btn btn-invisible js-clipboard-copy m-2 p-0 d-flex flex-justify-center flex-items-center" data-copy-feedback="Copied!" data-tooltip-direction="w" value="Update-Database -Context {nome_contexto} -StartupProject {nome_projeto_startup}" tabindex="0" role="button">
      <svg aria-hidden="true" height="16" viewBox="0 0 16 16" version="1.1" width="16" data-view-component="true" class="octicon octicon-copy js-clipboard-copy-icon">     
      </svg>
      <svg aria-hidden="true" height="16" viewBox="0 0 16 16" version="1.1" width="16" data-view-component="true" class="octicon octicon-check js-clipboard-check-icon color-fg-success d-none">
         
      </svg>
    </clipboard-copy>
  </div></div>
</li>
<li>
  <p dir="auto">Configurar a solution da aplicação no Visual Studio para iniciar vários projetos, exatamente com os mesmos projetos mostrados na figura abaixo:</p>
  <p dir="auto"><a target="_blank" rel="noopener noreferrer" href="https://github.com/raphamendes123/e-commerce/blob/main/docs/projeto.png">
    <img src="https://github.com/raphamendes123/e-commerce/blob/main/docs/projeto.png" alt="Selecionando os projetos para executar no visual studio" style="max-width: 100%;">
    </a>
  </p>
</li>
<li>
  <p dir="auto">
    <a target="_blank" rel="noopener noreferrer" href="https://github.com/raphamendes123/e-commerce/blob/main/docs/varios_projeto.png">
    <img src="https://github.com/raphamendes123/e-commerce/blob/main/docs/varios_projeto.png" alt="Startar a aplicação com N projetos" style="max-width: 100%;">
    </a>
  </p>
</li>
</ul>
<div class="markdown-heading" dir="auto"><h3 tabindex="-1" class="heading-element" dir="auto">Execução pelo Docker</h3><a id="user-content-execução-pelo-docker" class="anchor" aria-label="Permalink: Execução pelo Docker" href="#execução-pelo-docker"><svg class="octicon octicon-link" viewBox="0 0 16 16" version="1.1" width="16" height="16" aria-hidden="true"><path d="m7.775 3.275 1.25-1.25a3.5 3.5 0 1 1 4.95 4.95l-2.5 2.5a3.5 3.5 0 0 1-4.95 0 .751.751 0 0 1 .018-1.042.751.751 0 0 1 1.042-.018 1.998 1.998 0 0 0 2.83 0l2.5-2.5a2.002 2.002 0 0 0-2.83-2.83l-1.25 1.25a.751.751 0 0 1-1.042-.018.751.751 0 0 1-.018-1.042Zm-4.69 9.64a1.998 1.998 0 0 0 2.83 0l1.25-1.25a.751.751 0 0 1 1.042.018.751.751 0 0 1 .018 1.042l-1.25 1.25a3.5 3.5 0 1 1-4.95-4.95l2.5-2.5a3.5 3.5 0 0 1 4.95 0 .751.751 0 0 1-.018 1.042.751.751 0 0 1-1.042.018 1.998 1.998 0 0 0-2.83 0l-2.5 2.5a1.998 1.998 0 0 0 0 2.83Z"></path></svg></a></div>
<p dir="auto">A execução pelo Docker, mais especificamente pelo <a href="https://docs.docker.com/compose/" rel="nofollow">Docker Compose</a>, é muito mais simples e rápido do que anterior, pois não é necessário ter toda a stack necessária para executar a aplicação, mas somente o Docker. E esse setup rápido é um de seus benefícios, além da escalabilidade proporcionada e a maior facilidade de implantação da aplicação. Dado isso, basta seguir os seguintes passos:</p>
<ul dir="auto">
<li>
<p dir="auto">Obviamente, instalar o Docker habilitando o WSL2 e virtualização, principalmente se estiver no SO Windows;</p>
</li>
<li>
<p dir="auto">Abra o terminal no diretório <em>/deploy</em> e execute o seguinte comando:</p>
<div class="snippet-clipboard-content notranslate position-relative overflow-auto"><pre class="notranslate"><code>docker-compose -f docker-compose.yml up --build
</code></pre><div class="zeroclipboard-container">
    <clipboard-copy aria-label="Copy" class="ClipboardButton btn btn-invisible js-clipboard-copy m-2 p-0 d-flex flex-justify-center flex-items-center" data-copy-feedback="Copied!" data-tooltip-direction="w" value="docker-compose -f docker-compose.yml up --build" tabindex="0" role="button">
      <svg aria-hidden="true" height="16" viewBox="0 0 16 16" version="1.1" width="16" data-view-component="true" class="octicon octicon-copy js-clipboard-copy-icon">
    <path d="M0 6.75C0 5.784.784 5 1.75 5h1.5a.75.75 0 0 1 0 1.5h-1.5a.25.25 0 0 0-.25.25v7.5c0 .138.112.25.25.25h7.5a.25.25 0 0 0 .25-.25v-1.5a.75.75 0 0 1 1.5 0v1.5A1.75 1.75 0 0 1 9.25 16h-7.5A1.75 1.75 0 0 1 0 14.25Z"></path><path d="M5 1.75C5 .784 5.784 0 6.75 0h7.5C15.216 0 16 .784 16 1.75v7.5A1.75 1.75 0 0 1 14.25 11h-7.5A1.75 1.75 0 0 1 5 9.25Zm1.75-.25a.25.25 0 0 0-.25.25v7.5c0 .138.112.25.25.25h7.5a.25.25 0 0 0 .25-.25v-7.5a.25.25 0 0 0-.25-.25Z"></path>
</svg>
      <svg aria-hidden="true" height="16" viewBox="0 0 16 16" version="1.1" width="16" data-view-component="true" class="octicon octicon-check js-clipboard-check-icon color-fg-success d-none">
    <path d="M13.78 4.22a.75.75 0 0 1 0 1.06l-7.25 7.25a.75.75 0 0 1-1.06 0L2.22 9.28a.751.751 0 0 1 .018-1.042.751.751 0 0 1 1.042-.018L6 10.94l6.72-6.72a.75.75 0 0 1 1.06 0Z"></path>
</svg>
    </clipboard-copy>
  </div></div>
<p dir="auto"><strong>OBS.:</strong> <em>--build</em> é uma flag opcional que força a criação de todas as imagens docker.</p>
</li>
<li>
<p dir="auto">Para remover todos os containers docker gerados a partir das imagens docker basta usar o seguinte comando:</p>
<div class="snippet-clipboard-content notranslate position-relative overflow-auto"><pre class="notranslate"><code>docker-compose -f docker-compose.yml down
</code></pre><div class="zeroclipboard-container">
    <clipboard-copy aria-label="Copy" class="ClipboardButton btn btn-invisible js-clipboard-copy m-2 p-0 d-flex flex-justify-center flex-items-center" data-copy-feedback="Copied!" data-tooltip-direction="w" value="docker-compose -f docker-compose.yml down" tabindex="0" role="button">
      <svg aria-hidden="true" height="16" viewBox="0 0 16 16" version="1.1" width="16" data-view-component="true" class="octicon octicon-copy js-clipboard-copy-icon">
    <path d="M0 6.75C0 5.784.784 5 1.75 5h1.5a.75.75 0 0 1 0 1.5h-1.5a.25.25 0 0 0-.25.25v7.5c0 .138.112.25.25.25h7.5a.25.25 0 0 0 .25-.25v-1.5a.75.75 0 0 1 1.5 0v1.5A1.75 1.75 0 0 1 9.25 16h-7.5A1.75 1.75 0 0 1 0 14.25Z"></path><path d="M5 1.75C5 .784 5.784 0 6.75 0h7.5C15.216 0 16 .784 16 1.75v7.5A1.75 1.75 0 0 1 14.25 11h-7.5A1.75 1.75 0 0 1 5 9.25Zm1.75-.25a.25.25 0 0 0-.25.25v7.5c0 .138.112.25.25.25h7.5a.25.25 0 0 0 .25-.25v-7.5a.25.25 0 0 0-.25-.25Z"></path>
</svg>
      <svg aria-hidden="true" height="16" viewBox="0 0 16 16" version="1.1" width="16" data-view-component="true" class="octicon octicon-check js-clipboard-check-icon color-fg-success d-none">
    <path d="M13.78 4.22a.75.75 0 0 1 0 1.06l-7.25 7.25a.75.75 0 0 1-1.06 0L2.22 9.28a.751.751 0 0 1 .018-1.042.751.751 0 0 1 1.042-.018L6 10.94l6.72-6.72a.75.75 0 0 1 1.06 0Z"></path>
</svg>
    </clipboard-copy>
  </div></div>
</li>
</ul>
<p dir="auto"><strong>OBS.:</strong> pode ocorrer de ao rodar pelo Docker através do docker-compose as API's subam primeiro do que o broker de mensagens (Event Bus). Isso acaba levando nas API's não se conectarem broker de mensagens e ocasionar inconsistências na aplicação. Caso isto ocorra basta restartar as instâncias dos containers de todas as API's manualmente pelo Docker e voltará a funcionar corretamente.</p>
<hr>
  <div class="markdown-heading" dir="auto"><h2 tabindex="-1" class="heading-element" dir="auto">Referências</h2><a id="user-content-referências" class="anchor" aria-label="Permalink: Referências" href="#referências"><svg class="octicon octicon-link" viewBox="0 0 16 16" version="1.1" width="16" height="16" aria-hidden="true">
    <path d="m7.775 3.275 1.25-1.25a3.5 3.5 0 1 1 4.95 4.95l-2.5 2.5a3.5 3.5 0 0 1-4.95 0 .751.751 0 0 1 .018-1.042.751.751 0 0 1 1.042-.018 1.998 1.998 0 0 0 2.83 0l2.5-2.5a2.002 2.002 0 0 0-2.83-2.83l-1.25 1.25a.751.751 0 0 1-1.042-.018.751.751 0 0 1-.018-1.042Zm-4.69 9.64a1.998 1.998 0 0 0 2.83 0l1.25-1.25a.751.751 0 0 1 1.042.018.751.751 0 0 1 .018 1.042l-1.25 1.25a3.5 3.5 0 1 1-4.95-4.95l2.5-2.5a3.5 3.5 0 0 1 4.95 0 .751.751 0 0 1-.018 1.042.751.751 0 0 1-1.042.018 1.998 1.998 0 0 0-2.83 0l-2.5 2.5a1.998 1.998 0 0 0 0 2.83Z">    
    </path>
  </svg>
</a>
</div>
<p dir="auto">Abaixo estão os links das principais fontes para realização desse projeto, com ênfase no curso da plataforma de cursos 
  <p>
  <a href="https://desenvolvedor.io/" rel="nofollow">  Desenvolvedor.IO (A MELHOR PLATAFORMA DE CURSOS .NET) - Eduardo Pires</a>.</p>
  </p>
<ul dir="auto">
<li><a href="https://desenvolvedor.io/curso-online-asp-net-core-enterprise-applications" rel="nofollow">ASP.NET Core Enterprise Applications</a>;</li>
<li><a href="https://github.com/desenvolvedor-io/dev-store">Dev-Store Github Repo</a>;</li> 
<li><a href="https://docs.microsoft.com/en-us/dotnet/architecture/microservices/" rel="nofollow">.NET Microservices: Architecture for Containerized .NET Applications</a>;</li>
<li><a href="https://docs.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/microservice-application-design" rel="nofollow">Design a microservice-oriented application</a>;</li>
<li><a href="https://docs.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/data-driven-crud-microservice" rel="nofollow">Creating a simple data-driven CRUD microservice</a>;</li>
<li><a href="https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice" rel="nofollow">Design a DDD-oriented microservice</a>.</li>
<li><a href="https://www.rabbitmq.com/" rel="nofollow">RabbitMQ</a>.</li>
<li><a href="https://learn.microsoft.com/en-us/aspnet/core/grpc/?view=aspnetcore-8.0" rel="nofollow">gRPC on .NET</a>.</li>
<li><a href="https://www.docker.com/" rel="nofollow">Docker</a>.</li>
<li><a href="https://hub.docker.com/_/nginx" rel="nofollow">NGINX</a>.</li>
</ul>
</article>

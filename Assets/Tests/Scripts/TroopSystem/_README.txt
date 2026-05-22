Descrição do cenário:

Teste da movimentação do personagem com limite de alcançe

Na cena estão presentes: 
    -> Selection manager
    -> Input manager

O "personagem" (cubo) utiliza:
    -> Selectable Entity 
    -> Movement
    -> Reach
    -> Visible Reach

O que as 3 classes novas fazem:

/ Movement:
    - Tem um método "MoveTroop":
        -> Recebe uma posição vector3
        -> Chama "IsInReach" de Reach
        -> Executa procedimento de movimentação (no momento só modifica o transform do objeto para a posição alvo "teleportando")
/ Reach:
    - Define o raio de alcançe do objeto em que está presentes
    - Tem um método "IsInReach":
        -> Recebe uma posição
        -> Realiza as medidas minimas e maximas dos eixos, partindo do centro do objeto, somando ou subtraindo o alcançe definido (considera alcançe como um cubo)
        -> retorna um boleano se a posição recebida está dentro do cubo definido

/ Visible reach:
    - Serve para debugar Movement e Reach, criando uma representação visual de Reach e implementando um raycast para pegar a posição do mouse e chamar o metodo MoveTroop de Movement
    - Um material foi criado na pasta para ajudar a visualizar quando o personagem está "selecionado" além dos métodos fornecidos por Selectable Entity
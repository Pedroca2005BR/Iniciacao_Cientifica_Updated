# V10 — escritório mobiliado e texturizado

Base V9 preservada. Foram adicionados dois postos dentro da sala existente: mesas em L, cadeiras giratórias, computadores com monitor, gabinete, teclado e mouse. Quadro na parede do fundo com título ORDEM DE SERVIÇO, atividades, responsáveis e situações fictícias.

A mesa foi convertida diretamente do arquivo 5.125.E253.IGS fornecido, com normalização para 0,74 m de altura, correção de orientação e simplificação da malha. O CAD da sala de telemarketing também foi convertido para inspeção, mas seu conjunto completo não foi incorporado: sua imagem orientou a composição, e as cadeiras e computadores foram modelados de forma simplificada. O mobiliário está em 16_Escritorio_Mobiliario.

Texturas criadas para este cenário: madeira, tecido da cadeira, tela do computador e quadro. Todas estão empacotadas no arquivo Blender e disponíveis como PNG na pasta Texturas. O FBX foi exportado com imagens incorporadas; a reimportação foi conferida no Blender. Na Unity, poderá ser necessário extrair os materiais/texturas e ajustar os shaders conforme o pipeline do projeto. Não foi realizado teste de desempenho em RV nem configuração de interação.

O quadro e as telas são imagens estáticas. Para mudar os textos, edite ou substitua Quadro_ordem_servico.png e Tela_computador.png e atualize as imagens no material. O script Scripts/texturas_escritorio_v10.py permite regenerar as imagens.

O mobiliário do escritório soma aproximadamente 175 mil triângulos, principalmente pelas duas mesas CAD. A geometria foi reduzida, mas ainda deve ser avaliada junto ao restante do cenário no equipamento de RV escolhido.

Verificações: limites do mobiliário dentro da sala, apoio dos monitores/teclados/mouses nos tampos, quatro imagens empacotadas no Blender e recuperadas na reimportação do FBX. Galpão, sala, área livre de RV e ferramentaria mantidos.


% читаем файл с матрицей смежности
fileID = fopen('D:\Учеба\DiceWars\CreatingGraph\CreatingGraph\bin\Debug\netcoreapp2.1\matr.txt', 'r');
formatSpec = '%d';
for i = 1:9
    formatSpec = strcat(formatSpec, ' %d');
end
sizeA = [10 10];
A = fscanf(fileID, formatSpec, sizeA);

% читаем данные для каждой из вершин
fileID = fopen('D:\Учеба\DiceWars\CreatingGraph\CreatingGraph\bin\Debug\netcoreapp2.1\regions_list.txt', 'r');
formatSpec = '%d %d %d';
sizeA = [10 3];
regionsList = fscanf(fileID, formatSpec, sizeA);

% проходимся по списку
nodenames2 = {};
for i = 1:10
    nodenames2 = {nodenames2, int2str(regionsList(i, 2))};% возле каждой вершины запишем число кубиков
end
nodenames2 = nodenames2';

G = graph(A, nodenames2);% получаем граф
plot(G)
# MonitoringSolution

## Запуск проекта

### 1. Клонируем репозиторий


```bash
git clone https://github.com/MackZ28/MonitoringSolution.git
cd MonitoringSolution
```

## Запуск в контейнерах(с использованием Docker Compose)

1. Необходимо чтобы на машине был установлен Docker(Docker Desktop, Compose)
2. Запустить проект из коря проекта(где находится `docker-compose.yml`)

   ```bash
   cd /path/to/MonitoringSolution
   ```
3. После успешного запуска должны работать 3 адреса: 
    - Backend: http://localhost:5000
    - Swagger backend: http://localhost:5000/swagger
    - Frontend: http://localhost:3000
4. Остановить контейнер: 
    ```bash
   docker-compose down
   ```

---

## Ручной запуск backend (без контейнеров)
Если необходимо запустить бэкенд без контейнера сперва нужно будет поднять БД, для ручной инициализации бд можно использовать скрипт:
```bash
  psql -U postgres -f database/init.sql
  ```
  предварительно создав объект базы данных и настроив строку подключения к БД в файле `backend/appsettings.json`.

  Далее: 
1. Установить зависимости(если нужно): 
  ```bash
   dotnet restore
   ```
2. Запустить backend:
    ```bash
   dotnet run
   ```
4. Swagger будет доступна после запуска на [http://localhost:5000/swagger](http://localhost:5000/swagger).

---

## Ручной запуск frontend (без контейнеров)

1. Перейдите в папку frontend:

   ```bash
   cd frontend
   ```

2. Установите зависимости:

   ```bash
   npm install
   ```

3. Запустите приложение:

   ```bash
   npm start
   ```
   Откроется http://localhost:3000/

4. При необходимости настройте переменные окружения (например, файл `.env`, если требуется явно задать адрес API):

   ```ini
   REACT_APP_API_URL=http://localhost:5000
   ```

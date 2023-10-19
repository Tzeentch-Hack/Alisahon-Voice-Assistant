# Alisahon-Voice-Assistant
Uz-NLP Challenge hackathon 

# Описание

Alisahon-Voice-Assistant - это голосовой помщник, выполненный в виде клиент серверного приложения. Особенность голосового помощника - это общение с пользователем на Узбекском языке и наличие функции обнаружения тревожной фразы в речи пользователя для вызова экстренной помощи. 

Проект был разработан в течении хакатона UzNLP Challange https://it-park.uz/ru/itpark/news/sostoitsya-hakaton-po-iskusstvennomu-intellektu-uz-nlp-challenge. По итогам оценок жюри проекту было присвоено 1-место.

# Детали реализации:

Серверная часть написана на Flask-Python.
Клиентская часть использует Unity-C#.

Процесс взаимодействия с пользователем разделён на несколько этапов.
Unity высылает Аудио на сервер. Аудио декодируется с помощью обученной на хакатоне модели распознавания голоса (использовался датасет https://commonvoice.mozilla.org/en/datasets). Декодированный текст переводится на узбекский языке и высылается обратно на клиент, где отображается в виде плашки чата, введённой пользователем.
Если пользователь высылает текст, а не аудио - то этот этап пропускается.
Далее текст переводится на английский язык и подаётся на вход в модель обработки естественного языка, которая выдаёт осмысленный (насколько это возможно для GPT) ответ.
Полученный от модели текст переводится на узбекский язык. 
Дальше этот текст озвучивается моделями синтеза и вокодера от Silero.
В конечном итоге текст и файл озвучки отправляются на клиент, где показываются в виде сообщения от голосового помощника.

# Установка:

Там есть файл requirements.txt, и может быть сработает
pip install requirements.txt
Но скорее всего не сработает и установить этот проект у вас не получится, потому что было использовано столько разных штук для решения разных задач, что 
вообще не понятно сейчас, как и в каком порядке нужно устанавливать зависимости и какие версии использовать.

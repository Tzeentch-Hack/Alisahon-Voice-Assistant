import io
import os
from pydub import AudioSegment

from flask import Flask, request, jsonify, make_response
from werkzeug.utils import secure_filename

app = Flask(__name__)

audios_path = 'audios'

def compose_response(action, question_text, answer_text, audio_url):
    response_body = jsonify(
        action=action,
        audioUrl=audio_url,
        questionText=question_text,
        answerText=answer_text
    )
    return response_body

@app.route("/")
def hello_world():
    print("path ", app.root_path, " ", app.instance_path)
    return "<p>Alisahon WEB API</p>"


@app.route("/getAnswerByText", methods=['POST', 'GET'])
def get_answer_by_text():
    if request.method == 'POST':
        data = request.get_json()
        text = data['text']
        if text != "":
            print("Get text ", text)
            response_body = compose_response("some action", "Here will be question text", "Here will be answer text",
                                             "AudioUrl")
            return make_response(response_body, 200)
        else:
            return make_response("text is empty", 400)
    if request.method == 'GET':
        pass


@app.route("/getAnswerByAudio", methods=['POST', 'GET'])
def get_answer_by_audio():
    if request.method == 'POST':
        file = request.files['audio']
        #mybytes = file.read()
        if file != '':
            filename = secure_filename(file.filename)
            print("Get file ", filename)
            #mp3file = AudioSegment.from_file(file.read(), format="mp3")
            #mp3file.save(os.path.join(app.root_path, audios_path, filename + ".mp3"))
            file.save(os.path.join(app.root_path, audios_path, filename + ".mp3"))
            response_body = compose_response("some action", "Here will be question text", "Here will be answer text",
                                             "AudioUrl")
            return make_response(response_body, 200)
        else:
            return make_response("file is empty", 400)
        #return make_response("error", 500)


if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000, debug=True)

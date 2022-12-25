import os

from flask import Flask, request, jsonify, make_response, send_from_directory, url_for
from werkzeug.utils import secure_filename

import synthesizer

app = Flask(__name__)
import lm_facebook
import train
audios_path = 'audios'


def compose_response(action, question_text, answer_text, audio_url):
    response_body = jsonify(
        action='alarm',
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
            # Здесь получим ответ от Игоря
            answer_text = lm_facebook.lm_answer(text)
            #answer_text = "Kaleysan, yaxshimisan, o'rto?"
            audio_path = synthesizer.make_audio_from_text(answer_text)
            audio_url = url_for("download", file_path=audio_path)
            print("url in answer " + audio_url)
            response_body = compose_response("some action", text, answer_text=answer_text,
                                             audio_url=audio_url)
            return make_response(response_body, 200)
        else:
            return make_response("text is empty", 400)
    if request.method == 'GET':
        pass


@app.route("/getAnswerByAudio", methods=['POST', 'GET'])
def get_answer_by_audio():
    if request.method == 'POST':
        file = request.files['audio']
        if file != '':
            filename = secure_filename(file.filename)
            print("Get file ", filename)
            fname = os.path.join(app.root_path, audios_path, filename + ".mp3")
            file.save(fname)
            fname_list = [fname]
            recognized_text = train.first_asr_model.transcribe(fname_list, batch_size=1)
            answer_text = lm_facebook.lm_answer(recognized_text)
            # answer_text = "Kaleysan, yaxshimisan, o'rto?"
            audio_path = synthesizer.make_audio_from_text(answer_text)
            audio_url = url_for("download", file_path=audio_path)
            print("url in answer " + audio_url)
            response_body = compose_response("some action", recognized_text, answer_text=answer_text,
                                             audio_url=audio_url)
            return make_response(response_body, 200)
        else:
            return make_response("file is empty", 400)


@app.route('/download/<path:file_path>', methods=['GET'])
def download(file_path):
    print("Sending file from ", file_path)
    directory = os.path.dirname(file_path)
    file_name = os.path.basename(file_path)
    return send_from_directory(directory=directory, path=file_name)


if __name__ == "__main__":
    synthesizer.initialize(app.root_path)
    app.run(host="0.0.0.0", port=5000, debug=True)

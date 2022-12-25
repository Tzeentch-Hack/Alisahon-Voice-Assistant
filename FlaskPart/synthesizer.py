import contextlib
import os
import wave

import torch
import torchaudio
import soundfile as sf
import time
from pydub import AudioSegment

model = ''

root_path = ''
answer_audios_path = 'answer_audios'


def initialize(root_path_main):
    torchaudio.set_audio_backend("soundfile")  # switch backend

    global root_path
    root_path = root_path_main

    language = 'uz'
    model_id = 'v3_uz'
    device = torch.device('cpu')

    global model
    model, example_text = torch.hub.load(repo_or_dir='snakers4/silero-models',
                                         model='silero_tts',
                                         language=language,
                                         speaker=model_id)
    model.to(device)


def write_wave(path, audio, sample_rate):
    """Writes a .wav file.
    Takes path, PCM audio data, and sample rate.
    """
    with contextlib.closing(wave.open(path, 'wb')) as wf:
        wf.setnchannels(1)
        wf.setsampwidth(2)
        wf.setframerate(sample_rate)
        wf.writeframes(audio)


def make_audio_from_text(text):
    example_text = text + "..."
    sample_rate = 48000
    speaker = 'dilnavoz'

    #audio_paths = model.save_wav(text=example_text,
    #                             speaker=speaker,
    #                             sample_rate=sample_rate,
    #                             )

    audio_tensor = model.apply_tts(example_text, speaker=speaker)
    file_name = "answer_file" + str(time.time_ns()) + ".wav"
    audio_path = os.path.join(root_path, answer_audios_path, file_name)
    print(audio_path)
    sf.write(audio_path, audio_tensor, sample_rate, format='wav')
    AudioSegment.from_wav("answer_audios/" + file_name).export("answer_audios/" + "converted_" + file_name, format="mp3")
    #torchaudio.save(src=audio_tensor.data, sample_rate=sample_rate, filepath=audio_path)
    #shutil.move(audio_paths, audio_path)
    #return audio_path
    return "answer_audios/" + "converted_" + file_name


#initialize('/home/i_gore/PycharmProjects/Alisaxon')
#make_audio_from_text("Kaleysan, yaxshimisan, o'rto?")

#print(sf.available_subtypes('wav'))
#print(sf.available_subtypes('mp3'))
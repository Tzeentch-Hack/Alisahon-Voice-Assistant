import librosa
import nemo
import nemo.collections.asr as nemo_asr


import json
import os
from datasets import load_dataset

from huggingface_hub.commands.user import login

login("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")

ds = load_dataset("mozilla-foundation/common_voice_11_0", "uz", use_auth_token=True)


def prepare_dataset(batch):
    """Function to preprocess the dataset with the .map method"""
    transcription = batch["sentence"]
    if transcription.startswith('"') and transcription.endswith('"'):
        # we can remove trailing quotation marks as they do not affect the transcription
        transcription = transcription[1:-1]
    transcription = transcription.replace('\u2019', "'")
    transcription = transcription.replace('\u2018', "'")
    transcription = transcription.replace('\u02bb', "'")
    transcription = transcription.replace('\u00ac', "")
    transcription = transcription.replace('\u00f5', "o'")
    transcription = transcription.replace('\u00b4', "'")
    transcription = transcription.replace('\u201e', "")
    transcription = transcription.replace('\u2022', "")
    transcription = transcription.replace('\u2013', "")
    transcription = transcription.replace('\u2014', "")
    transcription = transcription.replace('\u2026', "")
    transcription = transcription.replace('\u2018', "")
    transcription = transcription.replace('\u2026', "")
    transcription = transcription.replace('\u201c', "")
    transcription = transcription.replace('\u00ab', "")
    transcription = transcription.replace('\u00bb', "")
    transcription = transcription.replace('\u2714', "")
    transcription = transcription.replace('\u00b4', "'")
    transcription = transcription.replace('\u02bc', "'")
    transcription = transcription.replace('\u201d', "")
    transcription = transcription.replace('\u0410', "a")
    transcription = transcription.replace('\u01b6', "z")
    transcription = transcription.replace('\u045e', "u")
    transcription = transcription.replace('\"', "")
    transcription = transcription.replace('\u0440', "r")
    transcription = transcription.replace('\u0438', "i")
    transcription = transcription.replace('\u042d', "e")
    transcription = transcription.replace('\u0431', "b")
    transcription = transcription.replace('\u0442', "t")
    transcription = transcription.replace('\u0430', "a")
    transcription = transcription.replace('\u0441', "s")
    transcription = transcription.replace('\u0437', "z")
    transcription = transcription.replace('\u043a', "k")
    transcription = transcription.replace('\u043a', "k")
    transcription = transcription.replace('\u043b', "l")
    transcription = transcription.replace('\u0433', "g")
    transcription = transcription.replace('\u043d', "n")
    transcription = transcription.replace('\u043c', "m")
    transcription = transcription.replace('\u0443', "u")
    transcription = transcription.replace('\u0411', "b")
    transcription = transcription.replace('\u044e', "yu")
    transcription = transcription.replace('\u0493', "g'")
    transcription = transcription.replace('\u049a', "q")
    transcription = transcription.replace('\u049b', "q")
    transcription = transcription.replace('\u044b', "i")
    transcription = transcription.replace('\u0448', "sh")
    transcription = transcription.replace('\u0434', "d")
    transcription = transcription.replace('\u04b3', "h")
    transcription = transcription.replace('\u0423', "u")
    transcription = transcription.replace('\u0439', "y")
    transcription = transcription.replace('\u0422', "t")
    transcription = transcription.replace('\u0299', "b")
    transcription = transcription.replace('\u00f2', "o'")
    transcription = transcription.replace('\u00d2', "o'")
    transcription = transcription.replace('\u015f', "sh")
    transcription = transcription.replace('\u0123', "g'")
    transcription = transcription.replace('\u05b9', "")
    transcription = transcription.replace('\u2642', "")
    transcription = transcription.replace('\u263a', "")
    transcription = transcription.replace('\ufe0f', "")
    transcription = transcription.replace('\u2705', "")
    batch["sentence"] = transcription
    return batch


ds = ds.map(prepare_dataset, desc="preprocess dataset")


# Function to build a manifest
def build_manifest(dataset_part, manifest_path):
    with open(manifest_path, 'w') as fout:
        for i, batch in enumerate(dataset_part):
            audio_path = batch['path']
            base_name = os.path.basename(audio_path)
            parent_folder = os.path.dirname(audio_path)
            dirs = os.listdir(parent_folder)
            my_dir = dirs[0]
            ready_path = os.path.join(parent_folder, my_dir, base_name)
            duration = librosa.core.get_duration(filename=ready_path)

            # Write the metadata to the manifest
            metadata = {
                "audio_filepath": ready_path,
                "duration": duration,
                "text": batch['sentence']
            }
            json.dump(metadata, fout)
            fout.write('\n')
            if i % 1000 == 0:
                print('i:', i)


# Building Manifests
print("******")
train_manifest = 'train_manifest.json'
build_manifest(ds['train'], train_manifest)
print("Training manifest created.")

val_manifest = 'val_manifest.json'
build_manifest(ds['validation'], val_manifest)
print('gun Done')
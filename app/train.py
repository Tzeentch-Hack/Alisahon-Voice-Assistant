from ruamel.yaml import YAML
import pytorch_lightning as pl
from pprint import pprint
from omegaconf import DictConfig
import nemo
import torch
import nemo.collections.asr as nemo_asr
import logging
logging.getLogger('nemo_logger').setLevel(logging.ERROR)

config_path = 'lightning_logs/version_8/hparams.yaml'
device = 'cuda' if torch.cuda.is_available() else 'cpu'

yaml = YAML(typ='safe')
with open(config_path) as f:
    params = yaml.load(f)
#pprint(params)

#trainer = pl.Trainer(devices=1, accelerator='gpu', max_epochs=15)
params['cfg']['train_ds']['manifest_filepath'] = 'train_manifest.json'
params['cfg']['validation_ds']['manifest_filepath'] = 'val_manifest.json'
#first_asr_model = nemo_asr.models.EncDecCTCModel(cfg=DictConfig(params['model']), trainer=trainer)
#first_asr_model.save_to('model.nemo')
#first_asr_model = nemo_asr.models.EncDecCTCModel.restore_from('model.nemo')
first_asr_model = nemo_asr.models.EncDecCTCModel.load_from_checkpoint('lightning_logs/version_8/checkpoints/epoch=14-step=22320.ckpt')
first_asr_model.setup_training_data(train_data_config=params['cfg']['train_ds'])
first_asr_model.setup_validation_data(val_data_config=params['cfg']['validation_ds'])
#first_asr_model.hparams.lr = 6.918309709189363e-05
#trainer.fit(first_asr_model)
#first_asr_model.save_to('modelv2.nemo')

#paths2audio_files = ['common_voice_uz_30208944.mp3',
#'common_voice_uz_30209022.mp3',
#'common_voice_uz_30209023.mp3',
#'common_voice_uz_30209024.mp3',
#'common_voice_uz_30209025.mp3',]
#print(first_asr_model.transcribe(paths2audio_files=paths2audio_files, batch_size=5))

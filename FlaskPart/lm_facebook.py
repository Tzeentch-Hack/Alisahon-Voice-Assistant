import torch
from transformers import BlenderbotTokenizer, BlenderbotForConditionalGeneration
from googletrans import Translator

model_name = 'facebook/blenderbot-400M-distill'
tokenizer = BlenderbotTokenizer.from_pretrained(model_name)
model = BlenderbotForConditionalGeneration.from_pretrained(model_name)

device = 'cuda' if torch.cuda.is_available() else 'cpu'
model = model.to(device)

translator = Translator()


def uz_find_triggers(message):
    query = True
    ans_dict = {
        'internet': False,
        'app': False,
        'alarm': False,
        'message': None
    }
    if query:
        if 'internet' in message.lower() or 'internetda' in message.lower() or 'internetga' in message.lower():
            place = message.find('top')
            print('here place:', place)
            if place == -1:
                new_place = message.find('internet')
                print('new_place:', new_place)
                if 'internetda' in message.lower() or 'internetga' in message.lower():
                    ans_dict['message'] = message[new_place+10:]
                    ans_dict['internet'] = True
                    return ans_dict
                else:
                    ans_dict['message'] = message[new_place+8:]
                    ans_dict['internet'] = True
                    return ans_dict
            ans_dict['message'] = message[place+3:]
            ans_dict['internet'] = True
            return ans_dict
        if 'ilova' in message.lower() or 'ilovani' in message.lower():
            place = message.find('top')
            if place == -1:
                new_place = message.find('ilova')
                if 'ilovani' in message.lower():
                    ans_dict['message'] = message[new_place + 7:]
                    ans_dict['app'] = True
                    return ans_dict
                else:
                    ans_dict['message'] = message[new_place + 5:]
                    ans_dict['app'] = True
                    return ans_dict
            ans_dict['message'] = message[place+3:]
            ans_dict['app'] = True
            return ans_dict
        if "mazzam yo'q" in message.lower():
            ans_dict['alarm'] = True
            return ans_dict
    return ans_dict




def en_find_triggers(message):
    pass


def lm_answer(message):
    d = uz_find_triggers(message)
    if d['message'] is not None:
        if d['message'][0] == ' ':
            d['message'] = d['message'][1:]

    en_message = translator.translate(message, src='uz', dest='en').text
    inputs = tokenizer(en_message, return_tensors="pt")
    inputs = inputs.to(device)
    result = model.generate(**inputs, max_length=1000)
    en_answer = tokenizer.decode(result[0])
    en_answer = en_answer.replace('<s>', '')
    en_answer = en_answer.replace('</s>', '')
    uzb_answer = translator.translate(en_answer, src='en', dest='uz').text
    uzb_answer = uzb_answer.replace('.', '. ')
    uzb_answer = uzb_answer.replace('!', '! ')
    uzb_answer = uzb_answer.replace('?', '? ')
    if uzb_answer[-1] == ' ':
        uzb_answer = uzb_answer[:-1]
    return uzb_answer, d


#print(lm_answer("Alisaxon, mazzam yo'q"))

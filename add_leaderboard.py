import requests
import argparse
# from dotenv import load_dotenv
import os
import sys
from google_auth_oauthlib.flow import InstalledAppFlow
from googleapiclient.discovery import build
from googleapiclient.errors import HttpError


GAME_CENTER_API_KEY_NAME = 'GAME_CENTER_API_KEY'
GOOGLE_PLAY_APP_ID = '711191715930'

parser = argparse.ArgumentParser()
parser.add_argument('name', help='the friendly name of the leaderboard')
# parser.add_argument('ios_id', help='the Game Center ID of the leaderboard')
args = parser.parse_args()

# load_dotenv()

# game_center_key = os.environ.get(GAME_CENTER_API_KEY_NAME)
if (not os.path.isfile('client_secret.json')):
    sys.exit(
        f'Must configure a client_secret.json')

google_play_data = {
    "kind": "gamesConfiguration#leaderboardConfiguration",
    #   "token": string,
    #   "id": string,
    "scoreOrder": 'SMALLER_IS_BETTER',
    #   "scoreMin": long,
    #   "scoreMax": long,
    "draft": {
        "kind": "gamesConfiguration#leaderboardConfigurationDetail",
        "name": {
            "kind": "gamesConfiguration#localizedStringBundle",
            "translations": [
                {
                    "kind": "gamesConfiguration#localizedString",
                    "locale": 'en_US',
                    "value": args.name
                }
            ]
        },
        # "iconUrl": string,
        # "sortRank": integer,
        "scoreFormat": {
            "numberFormatType": 'TIME_DURATION',
            #   "suffix": {
            #     "zero": {
            #       "kind": "gamesConfiguration#localizedStringBundle",
            #       "translations": [
            #         {
            #           "kind": "gamesConfiguration#localizedString",
            #           "locale": string,
            #           "value": string
            #         }
            #       ]
            #     },
            #     "one": {
            #       "kind": "gamesConfiguration#localizedStringBundle",
            #       "translations": [
            #         {
            #           "kind": "gamesConfiguration#localizedString",
            #           "locale": string,
            #           "value": string
            #         }
            #       ]
            #     },
            #     "two": {
            #       "kind": "gamesConfiguration#localizedStringBundle",
            #       "translations": [
            #         {
            #           "kind": "gamesConfiguration#localizedString",
            #           "locale": string,
            #           "value": string
            #         }
            #       ]
            #     },
            #     "few": {
            #       "kind": "gamesConfiguration#localizedStringBundle",
            #       "translations": [
            #         {
            #           "kind": "gamesConfiguration#localizedString",
            #           "locale": string,
            #           "value": string
            #         }
            #       ]
            #     },
            #     "many": {
            #       "kind": "gamesConfiguration#localizedStringBundle",
            #       "translations": [
            #         {
            #           "kind": "gamesConfiguration#localizedString",
            #           "locale": string,
            #           "value": string
            #         }
            #       ]
            #     },
            #     "other": {
            #       "kind": "gamesConfiguration#localizedStringBundle",
            #       "translations": [
            #         {
            #           "kind": "gamesConfiguration#localizedString",
            #           "locale": string,
            #           "value": string
            #         }
            #       ]
            #     }
            #   },
            #   "numDecimalPlaces": integer,
            #   "currencyCode": string
        }
    }
}

flow = InstalledAppFlow.from_client_secrets_file(
    'client_secret.json',
    scopes=['https://www.googleapis.com/auth/androidpublisher'])

credentials = flow.run_local_server(host='localhost',
                                    port=8080,
                                    authorization_prompt_message='Please visit this URL: {url}',
                                    success_message='The auth flow is complete; you may close this window.',
                                    open_browser=True)

with build('gamesConfiguration', 'v1configuration', credentials=credentials) as leaderboard_service:
    print('Calling Google Play API')
    configurations = leaderboard_service.leaderboardConfigurations()
    request = configurations.insert(
        applicationId=GOOGLE_PLAY_APP_ID, body=google_play_data)
    try:
        response = request.execute()
    except HttpError as e:
        print('Error response status code : {0}, reason : {1}'.format(
            e.resp.status, e.error_details))

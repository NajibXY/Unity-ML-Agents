Unity: 2022.3.14f1
Python 3.9.13

Before venv or conda :
py -3.9 -m venv venv

After venv:
python -m pip install --upgrade pip
pip install mlagents
pip3 install torch torchvision torchaudio
pip install protobuf==3.20.3
pip install packaging

Works fine! Hope so u too :D




mlagents-learn ml-agents/config/ppo/PushBlock.yaml --run-id=push_block_test_03

https://unityatscale.com/unity-version-control-guide/how-to-setup-unity-project-on-github/
# Experimentations with Unity's Machine Learning library: ML-Agents

## Author: [Najib El khadir](https://github.com/NajibXY)
## French Version [README-fr](https://github.com/NajibXY/Unity-ML-Agents/blob/master/README-fr.md)

## 1. Motivations

<figure text-align="right">
  <img align="right" src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/gifs/car_controller_agent/advanced_training.gif" width="400">
</figure>

To improve my skills in both Unity and Machine Learning, I decided to work on a personal project involving various experiments within reinforcement learning environments that I design simultaneously in the Unity engine.

This entails several development axes:
- Designing Scenes in Unity.
- Creating Controllers and Learning Agents in Unity.
- Designing and executing Machine Learning scenarios using the ML-Agents library.

As well as a short-term goal: to develop an agent capable of controlling a car and driving in any suitable Unity scene.

## 2. Technologies Used

![](https://skillicons.dev/icons?i=python,pytorch,anaconda,cs,unity)
- Python 3.9, PyTorch, Conda (for my personal environment)
- Python library ML-Agents 0.30.0
- C#, Unity 2022.3.14f1, [ML-Agents package](https://github.com/Unity-Technologies/ml-agents): make sure your Unity version is compatible with the package version.

### Configuring Your Python Environment

- You'll need to set up a Python environment (in my case, conda), preferably version 3.9.13 to avoid library compatibility issues.
- To install dependencies with conda, you can simply run:
  > conda create --name `<your_env_name>` --file requirements.txt
- Or if you're using pip:
  > pip install -r requirements.txt
- Then, you can test if the `mlagents-learn` command works. This command triggers learning and listens on a dedicated port for your Unity project window.

## 3. References
- [Setting up a Git repository for a Unity project](https://unityatscale.com/unity-version-control-guide/how-to-setup-unity-project-on-github/)
- [ML-Agents library documentation](https://unity-technologies.github.io/ml-agents/ML-Agents-Toolkit-Documentation/)
- [Tutorial on using ML-Agents in Unity](https://www.youtube.com/watch?v=zPFU30tbyKs&list=PLzDRvYVwl53vehwiN_odYJkPBzcqFw110&ab_channel=CodeMonkey)

## 4. Environment [Reach The Goal](https://github.com/NajibXY/Unity-ML-Agents/tree/master/unity/MLAgentsTests)

<img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/images/reach_the_goal_scene.png" width="600">

### Scene Set Up

- The key with learning scenarios is to design a suitable environment.
- For the agent whose goal is to reach its target, the scene was designed, based on an example from the ML-Agents library, to have a controllable `Basic` prefab whose goal is to reach a ball, in a enclosed environment.
- In the learning context, the agent can move freely in the environment along the <X,Z> axis. It receives a positive (e.g., negative) reward, and the learning episode ends when the agent collides with the walls (e.g., the ball).
- The logic of the agent's implementation is made possible through the `Agent` interface of the C# classes from the ML-Agent library.
- The script to attach to the agent can be viewed here [MoveToGoalBasic.cs](https://github.com/NajibXY/Unity-ML-Agents/blob/master/unity/MLAgentsTests/Assets/Scripts/MoveToGoalBasic.cs)
- The components related to the ML-Agents library are as follows: </br>
  <img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/images/reach_the_goal_agent_params.png" width="250">

### Learning Set Up

- Once your scene is suitable for learning, we can clone the environment multiple times to have multiple participating environments in parallel, thereby speeding up the learning process.
- Randomization for the spawn of the agent and ball at the beginning of each episode is also implemented in the script.
- Coloring the surface of the Scene also allows us to know which agent has recently succeeded (reaching the ball) or failed (hitting a wall).

### Starting Training
- To begin training, remove the model and set the default behavior in the `Behaviour Parameters` component of the agent.
- Then, execute the following command in the Python environment:
  > mlagents-learn --run-id={an_id}
- Next, start the scene in Unity.

#### Beginning of Training
<img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/gifs/reach_the_goal_agent/training_begins.gif" width="650">
At the beginning, the agent learns to move and tries random actions. Sometimes it hits a wall.

#### Middle of Training
<img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/gifs/reach_the_goal_agent/middle_of_training.gif" width="650">
From a few thousand steps onwards, the agent applies a heuristic that becomes more reliable over episodes.

#### After 50000~ steps
<img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/gifs/reach_the_goal_agent/end_of_training.gif" width="650">
After tens of thousands of learning steps, the agent has developed a solid strategy and learned to master movement towards the goal.

### Results
- The result is exported in a [results](https://github.com/NajibXY/Unity-ML-Agents/tree/master/results) folder.
- The result folder contains a lot of information that can be visualized using `Tensorboard`, as well as an [importable Brain](https://github.com/NajibXY/Unity-ML-Agents/blob/master/results/MoveToGoalWithParametersAndRandomization_03/MoveToGoalBasic.onnx) in Unity assets and usable in agent components.
- By loading the `.onnx` file into the model used by the agent and setting the behavior type to `Inference only`, we can observe a perfectly automated and reusable behavior:
<img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/gifs/reach_the_goal_agent/trained_brain.gif" width="650">

## 4. Environment [Car Controller](https://github.com/NajibXY/Unity-ML-Agents/tree/master/unity/CarTrackAgent) + some Imitation Learning

<img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/images/car_controller_scene.png" width="600">

### Scene Set Up

- Even more than for the `Reach_The_Goal` agent, it is very important here to design the most suitable learning environment possible.
- For the agent whose goal is to learn how to drive on a track, the scene was designed using multiple assets. This involved building a car and its various components, a circular track with barriers serving as a frame for training, as well as an ordered CheckPoint system around which learning rewards were designed.
- The agent can accelerate, decelerate, reverse, turn left, and turn right.
- If it collides with the barriers, it receives a negative reward.
- If it remains in contact with the barriers, it receives a tempered negative reward.
- If it reaches its next CheckPoint (order matters), it receives a positive reward.
- If it reaches the last CheckPoint (the finish line) and completes a lap, it receives a large positive reward.
- If it reaches an incorrect CheckPoint (backward), it receives a negative reward.
- The agent also has a tunable `Ray Perception Sensor 3D` component, allowing it to detect barriers and checkpoints.
- The script to attach to the agent can be viewed here [CarController.cs](https://github.com/NajibXY/Unity-ML-Agents/blob/master/unity/CarTrackAgent/Assets/Scripts/CarController.cs)
- The components related to the ML-Agents library are as follows: </br>
  <img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/images/car_controller_params.png" width="250">

### Learning Set Up

- As before, once your scene is suitable for learning, we can clone the environment multiple times to have multiple participating environments in parallel, thereby speeding up the learning process. However, here, we will simply multiply the `CarController` to have multiple cars interacting with the same environment.
- Care must be taken to disable collisions between cars, as my objective is closer to a TrackMania bot than F1.

### Use of a Demonstration Recorder

- To assist in the agent's learning, I used a Demonstration Recorder component to "record" a complete lap of the track, which will serve as the basis for training. I made some mistakes during my lap, intentional (or not?) to minimize the assistance provided by this demo.

#### YAML Configuration File
- In order to use the demo for training, a YAML configuration file must be written.
- I wrote an example configuration [here](https://github.com/NajibXY/Unity-ML-Agents/blob/master/config_custom/car_imitation.yaml), tune it as desired.

### Starting Training
- To begin training, remove the model and set the default behavior in the `Behaviour Parameters` component of the agent.
- Then, execute the following command in the Python environment:
  > mlagents-learn config_custom/car_imitation.yaml --run-id={an_id}
- Next, start the scene in Unity.

#### Beginning of Training
<img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/gifs/car_controller_agent/begin_of_training.gif" width="650">
At the beginning, the agent learns tentatively, learns to move forward, backward, and moves in all directions.

#### After 100,000 steps
<img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/gifs/car_controller_agent/after_begin_training.gif" width="650">
From a few hundred thousand steps onwards, the agent has almost learned to target the first checkpoints.

#### After 500,000 steps
<img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/gifs/car_controller_agent/mid_training.gif" width="650">
After a few hundred thousand learning steps, the agent starts to recognize its goal well and completes laps. Some agents do laps in reverse, others crash, which is reassuring regarding noise propagation in the model.

#### After 1,500,000 steps
<img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/gifs/car_controller_agent/advanced_training.gif" width="650">
After more than a million learning steps, the agent moves smoothly around the track. It has learned to do laps!

#### Observation
Beyond 2,000,000 steps, the agent enters a strange overfitting phase where it "regresses" and the observed behaviors are as intriguing as they are detrimental to the final result. Further experiments will be conducted to better understand this, even if it is a classic limitation of reinforcement learning algorithms.

### Results
- The result is exported in a [results](https://github.com/NajibXY/Unity-ML-Agents/tree/master/results) folder.
- The result folder contains a lot of information that can be visualized using `Tensorboard`, as well as a [CarControllerBrain](https://github.com/NajibXY/Unity-ML-Agents/blob/master/results/imitation_car_test_20/CarController/CarController-1499948.onnx) importable in Unity assets and usable in agent components.
- By loading the `.onnx` file into the model used by the agent and setting the behavior to `Inference only`, we can observe a perfectly automated and reusable behavior:
<img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/gifs/car_controller_agent/trained_brain_demo_game.gif" width="650">

### Objective
- The current objective is to develop an agent capable of driving on any Unity track (as long as the agent script exposes certain necessary functions of the ML-Agent package), so I need to train on several types of tracks simultaneously to have an adaptive resulting agent.
- Stay tuned!
<img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/gifs/car_controller_agent/trained_brain_demo_scene.gif" width="650">

</br></br>

## 5. Possible Improvements

- Train the CarController model on other tracks.
- Continue experiments with other deep reinforcement learning models.
- [...]

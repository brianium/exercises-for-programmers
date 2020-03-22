# madlib

I was VEXED by this exercise.

My goal is to solve these exercises, including the bonus challenges.

Clojure was the first language I solved this in, so I will document my feelings here.

## The problem statement:

> Create a simple mad-lib program that prompts for a noun, a verb, an adverb, and an adjective and injects those into a story that you create.

### Constraints
* Use a single output statement for this program
* If your language supports string inerpolation or string subtitution, use it to build up the output.

### Challenges
* Add more inputs to the program to expand the story.
* Implement a branching story, where the answers to questions determine how the story is constructed.

## Oh how you vex me exercise!

### The first vexation!

The example output:

```
Enter a noun: dog
Enter a verb: walk
Enter an adjective: blue
Enter an adverb: quickly
Do you walk your blue dog quickly? That's hilarious!
```

The fact that inputs are not rendered in the order they are gathered adds complexity. This also seems to break out against the typical path for madlibs, though I suppose there is no rule that says the madlib proctor cannot ask out of order.

I solved this by allowing a story specification to declare the order a particular of part of speech should be included into a story. Essentially specifiy it's position in the template:

```clojure
(s/def ::ordered-part-of-speech (s/tuple ::part-of-speech int?))
```

### The second vexation!

This could just be a problem with my ability to understand things. The final challenge says:

> Implement a branching story, where the answers to question determine how the story is *constructed*.

***Constructed***!?!?!?!?!? Does this mean answers to certain questions can alter the next question that is asked? Does it alter the template?!?!?!? I assumed the answer to both should be yes, and this made it trickier.

Functions are just so great, and by requiring parts of speech to be a function of previous answers, and allowing templates to be a function of selections and all answers, it was solved pretty elegantly IMO.

```clojure
(def interactive-parts [(pos :noun 1)
                        (fn [[_ value]]
                             (case value
                               "wizard"  [:noun 2]
                               "warrior" [:verb 2]
                               [:adjective 2]))])

(def interactive-templates
  {[:noun :noun] "I am a great %s!! I seek the ancient %s!"
   [:noun :verb] "I am a mighty %s!! I will %s evil where I find it!"
   [:noun :adjective] "I am just a %s trying to be %s."})

(defn interactive-story []
  (m/exec
   interactive-parts
   (fn [selections answers]
     (->> answers
          (mapv ffirst)
          (get interactive-templates)
          (format-coll selections)))))
```

## Stories

### The base scenario:

```
clj -m brianium.madlib 1

Enter a noun: dog
Enter a verb: walk
Enter an adjective: blue
Enter an adverb: quickly
Do you walk your blue dog quickly? That's hilarious!
```

### The extra inputs scenario:

```
clj -m brianium.madlib 2

Enter a noun: dog
Enter a verb: walk
Enter an adjective: blue
Enter an adverb: quickly
Enter a verb: blow
Enter a noun: chunks
Do you walk your blue dog quickly? I might blow chunks! That's hilarious!
```

### The branching scenario:

```
clj -m brianium.madlib 3

Enter a noun: wizard
Enter a noun: Staff Of Power
I am a great wizard!! I seek the ancient Staff Of Power!

clj -m brianium.madlib 3

Enter a noun: warrior
Enter a verb: smite
I am a mighty warrior!! I will smite evil where I find it!

clj -m brianium.madlib 3

Enter a noun: developer
Enter an adjective: decent
I am just a developer trying to be decent.
```

(ns brianium.simple-math.cli
  (:require [clojure.spec.alpha :as s]
            [clojure.string :as string]
            [brianium.simple-math.core :as core]
            [brianium.simple-math.input :as input]))

(defn prompt
  [text]
  (print (str text " "))
  (flush)
  (-> (read-line)
      (string/trim)))

(s/fdef prompt
  :args (s/cat :text string?)
  :ret  string?)

(defn- correction
  "Present the user with feedback about what is wrong and ask the question again"
  [state question]
  (println (core/feedback state))
  (prompt question))

(defn ask
  [question]
  (loop [{:keys [state value]} (-> question prompt input/parse)]
    (if (= :success state)
      value
      (recur (-> state
                 (correction question)
                 (input/parse))))))

(s/fdef ask
  :args (s/cat :question string?)
  :ret  float?)

(defn printall
  [lines]
  (doseq [line lines]
    (println line)))

(defn run []
  (->> [(ask "What is the first number?") (ask "What is the second number?")]
       (apply core/calculate)
       (map core/render)
       (printall)))

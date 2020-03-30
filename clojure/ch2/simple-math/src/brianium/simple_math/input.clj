(ns brianium.simple-math.input
  (:require [clojure.spec.alpha :as s]
            [brianium.simple-math.input.spec :as input]))

(defn make-parse-state
  [f]
  (if (< f 0)
    {:state :negative :value f}
    {:state :success :value f}))

(s/fdef make-parse-state
  :args (s/cat :f float?)
  :ret  ::input/state)

(defn parse
  [s]
  (try
    (make-parse-state (Float/parseFloat s))
    (catch Exception _
      {:state :failure})))

(s/fdef parse
  :args (s/cat :s string?)
  :ret  (s/tuple float? boolean?))

(defn success?
  [parse-state]
  (= :success (:state parse-state)))

(s/fdef success?
  :args (s/cat :state ::input/state)
  :ret  boolean?)
